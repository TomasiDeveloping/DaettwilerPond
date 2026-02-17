using Application.DataTransferObjects.FishingClub;
using Application.DataTransferObjects.FishingLicense;
using Application.DataTransferObjects.User;
using Application.Interfaces;
using Application.Models;
using Infrastructure.Documents;
using QuestPDF.Fluent;

namespace Infrastructure.Services;

// Service for creating PDF documents and handling email communication
public class PdfService(IUserRepository userRepository, IFishingRegulationRepository fishingRegulationRepository,
    IFishTypeRepository fishTypeRepository, ISwissQrBillService qrBillService,
    IFishingClubRepository fishingClubRepository, IEmailService emailService,
    IFishingLicenseRepository fishingLicenseRepository) : IPdfService
{

    // Create PDF document for member details
    public async Task<byte[]> CreateMemberPdfAsync()
    {
        var userWithAddresses = await userRepository.GetUsersWithAddressesAsync();
        var document = new MemberDocument(userWithAddresses);
        return document.GeneratePdf();
    }

    // Create PDF document for fishing rules
    public async Task<byte[]> CreateFishingRulesPdfAsync()
    {
        var fishingRules = await fishingRegulationRepository.GetFishingRegulationsAsync();
        var document = new FishingRulesDocument(fishingRules);
        return document.GeneratePdf();
    }

    // Create PDF document for fish open seasons
    public async Task<byte[]> CreateFishOpenSeasonPdfAsync()
    {
        var fishTypes = await fishTypeRepository.GetFishTypesAsync();
        var document = new FishOpenSeasonDocument(fishTypes);
        return document.GeneratePdf();
    }

    // Send fishing license bills to users and create licenses if needed
    public async Task<bool> SendFishingLicenseBillAsync(CreateFishingLicenseBillDto createFishingLicenseBillDto,
        string creatorEmail)
    {
        // Retrieve fishing club information
        var fishingClub = await fishingClubRepository.GetFishingClubsAsync();

        // Iterate through user IDs and send bills
        foreach (var userId in createFishingLicenseBillDto.UserIds)
        {
            var userWithAddress = await userRepository.GetUserWithAddressByUserId(userId);
            var fishingLicenseBill = CreateFishingLicenseBill(userWithAddress, fishingClub.FirstOrDefault()!,
                createFishingLicenseBillDto.LicenseYear, DateTime.Now);
            var qrBill = qrBillService.CreateFishingLicenseBill(fishingLicenseBill);
            var document = new FishingLicenseBillDocument(qrBill, fishingLicenseBill);
            var invoice = document.GeneratePdf();

            // Send email with the fishing license bill
            await emailService.SendFishingLicenseBillAsync(createFishingLicenseBillDto.LicenseYear,
                userWithAddress.Email, createFishingLicenseBillDto.EmailMessage, invoice);

            // Create fishing license if needed
            if (!createFishingLicenseBillDto.CreateLicense) continue;
            var createLicenseDto = new CreateFishingLicenseDto
            {
                ExpiresOn = new DateTime(createFishingLicenseBillDto.LicenseYear, 12, 31),
                IsActive = true,
                IsPaid = false,
                UserId = userId,
                Year = createFishingLicenseBillDto.LicenseYear
            };
            await fishingLicenseRepository.CreateFishingLicenseAsync(createLicenseDto, creatorEmail);
        }

        return true;
    }

    // Retrieve PDF invoice for a user's fishing license
    public async Task<byte[]> GetUserFishingLicenseInvoiceAsync(Guid fishingLicenseId)
    {
        var fishingLicense = await fishingLicenseRepository.GetFishingLicenseAsync(fishingLicenseId);
        var fishingClub = await fishingClubRepository.GetFishingClubsAsync();
        var userWithAddress = await userRepository.GetUserWithAddressByUserId(fishingLicense.UserId);
        var fishingLicenseBill = CreateFishingLicenseBill(userWithAddress, fishingClub.FirstOrDefault()!,
            fishingLicense.Year, fishingLicense.CreatedAt);
        var qrBill = qrBillService.CreateFishingLicenseBill(fishingLicenseBill);
        var document = new FishingLicenseBillDocument(qrBill, fishingLicenseBill);
        return document.GeneratePdf();
    }

    // Create a FishingLicenseBill object based on user and fishing club information
    private static FishingLicenseBill CreateFishingLicenseBill(UserWithAddressDto userWithAddressDto,
        FishingClubDto fishingClubDto, int licenseYear, DateTime invoiceDate)
    {
        return new FishingLicenseBill
        {
            // Populate FishingLicenseBill properties based on user and fishing club data
            Amount = fishingClubDto.LicensePrice,
            CreditorName = fishingClubDto.BillAddressName,
            CreditorAddress = fishingClubDto.BillAddress,
            CreditorHouseNumber = fishingClubDto.BillHouseNumber,
            CreditorCity = fishingClubDto.BillCity,
            CreditorPostalCode = fishingClubDto.BillPostalCode,
            DebtorName = $"{userWithAddressDto.FirstName} {userWithAddressDto.LastName}",
            DebtorAddress = userWithAddressDto.Address.Street,
            DebtorHouseNumber = userWithAddressDto.Address.HouseNumber,
            DebtorPostalCode = userWithAddressDto.Address.PostalCode,
            DebtorCity =  userWithAddressDto.Address.City,
            LicenseYear = licenseYear.ToString(),
            ReferenceMessage =
                $"Fischerkarte {licenseYear}, {userWithAddressDto.FirstName} {userWithAddressDto.LastName}",
            FishingClubName = fishingClubDto.Name,
            Account = fishingClubDto.IbanNumber,
            InvoiceDate = invoiceDate
        };
    }
}