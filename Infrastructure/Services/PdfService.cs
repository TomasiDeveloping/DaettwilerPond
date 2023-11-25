using Application.DataTransferObjects.FishingClub;
using Application.DataTransferObjects.FishingLicense;
using Application.DataTransferObjects.User;
using Application.Interfaces;
using Application.Models;
using Infrastructure.Documents;
using QuestPDF.Fluent;

namespace Infrastructure.Services;

public class PdfService(IUserRepository userRepository, IFishingRegulationRepository fishingRegulationRepository,
    IFishTypeRepository fishTypeRepository, ISwissQrBillService qrBillService,
    IFishingClubRepository fishingClubRepository, IEmailService emailService,
    IFishingLicenseRepository fishingLicenseRepository) : IPdfService
{
    public async Task<byte[]> CreateMemberPdfAsync()
    {
        var userWithAddresses = await userRepository.GetUsersWithAddressesAsync();
        var document = new MemberDocument(userWithAddresses);
        return document.GeneratePdf();
    }

    public async Task<byte[]> CreateFishingRulesPdfAsync()
    {
        var fishingRules = await fishingRegulationRepository.GetFishingRegulationsAsync();
        var document = new FishingRulesDocument(fishingRules);
        return document.GeneratePdf();
    }

    public async Task<byte[]> CreateFishOpenSeasonPdfAsync()
    {
        var fishTypes = await fishTypeRepository.GetFishTypesAsync();
        var document = new FishOpenSeasonDocument(fishTypes);
        return document.GeneratePdf();
    }

    public async Task<bool> SendFishingLicenseBillAsync(CreateFishingLicenseBillDto createFishingLicenseBillDto,
        string creatorEmail)
    {
        var fishingClub = await fishingClubRepository.GetFishingClubsAsync();
        foreach (var userId in createFishingLicenseBillDto.UserIds)
        {
            var userWithAddress = await userRepository.GetUserWithAddressByUserId(userId);
            var fishingLicenseBill = CreateFishingLicenseBill(userWithAddress, fishingClub.FirstOrDefault()!,
                createFishingLicenseBillDto.LicenseYear, DateTime.Now);
            var qrBill = qrBillService.CreateFishingLicenseBill(fishingLicenseBill);
            var document = new FishingLicenseBillDocument(qrBill, fishingLicenseBill);
            var invoice = document.GeneratePdf();
            await emailService.SendFishingLicenseBillAsync(createFishingLicenseBillDto.LicenseYear,
                userWithAddress.Email, createFishingLicenseBillDto.EmailMessage, invoice);
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

    private static FishingLicenseBill CreateFishingLicenseBill(UserWithAddressDto userWithAddressDto,
        FishingClubDto fishingClubDto, int licenseYear, DateTime invoiceDate)
    {
        return new FishingLicenseBill
        {
            Amount = fishingClubDto.LicensePrice,
            CreditorName = fishingClubDto.BillAddressName,
            CreditorAddress = fishingClubDto.BillAddress,
            CreditorCity = $"{fishingClubDto.BillPostalCode} {fishingClubDto.BillCity}",
            DebtorName = $"{userWithAddressDto.FirstName} {userWithAddressDto.LastName}",
            DebtorAddress = $"{userWithAddressDto.Address.Street} {userWithAddressDto.Address.HouseNumber}",
            DebtorCity = $"{userWithAddressDto.Address.PostalCode} {userWithAddressDto.Address.City}",
            LicenseYear = licenseYear.ToString(),
            ReferenceMessage =
                $"Fischerkarte {licenseYear}, {userWithAddressDto.FirstName} {userWithAddressDto.LastName}",
            FishingClubName = fishingClubDto.Name,
            Account = fishingClubDto.IbanNumber,
            InvoiceDate = invoiceDate
        };
    }
}