using Application.DataTransferObjects.FishingClub;
using Application.DataTransferObjects.FishingLicense;
using Application.DataTransferObjects.User;
using Application.Interfaces;
using Application.Models;
using Infrastructure.Documents;
using QuestPDF.Fluent;

namespace Infrastructure.Services;

public class PdfService : IPdfService
{
    private readonly IEmailService _emailService;
    private readonly IFishingClubRepository _fishingClubRepository;
    private readonly IFishingLicenseRepository _fishingLicenseRepository;
    private readonly IFishingRegulationRepository _fishingRegulationRepository;
    private readonly IFishTypeRepository _fishTypeRepository;
    private readonly ISwissQrBillService _qrBillService;
    private readonly IUserRepository _userRepository;

    public PdfService(IUserRepository userRepository, IFishingRegulationRepository fishingRegulationRepository,
        IFishTypeRepository fishTypeRepository, ISwissQrBillService qrBillService,
        IFishingClubRepository fishingClubRepository, IEmailService emailService,
        IFishingLicenseRepository fishingLicenseRepository)
    {
        _userRepository = userRepository;
        _fishingRegulationRepository = fishingRegulationRepository;
        _fishTypeRepository = fishTypeRepository;
        _qrBillService = qrBillService;
        _fishingClubRepository = fishingClubRepository;
        _emailService = emailService;
        _fishingLicenseRepository = fishingLicenseRepository;
    }

    public async Task<byte[]> CreateMemberPdfAsync()
    {
        var userWithAddresses = await _userRepository.GetUsersWithAddressesAsync();
        var document = new MemberDocument(userWithAddresses);
        return document.GeneratePdf();
    }

    public async Task<byte[]> CreateFishingRulesPdfAsync()
    {
        var fishingRules = await _fishingRegulationRepository.GetFishingRegulationsAsync();
        var document = new FishingRulesDocument(fishingRules);
        return document.GeneratePdf();
    }

    public async Task<byte[]> CreateFishOpenSeasonPdfAsync()
    {
        var fishTypes = await _fishTypeRepository.GetFishTypesAsync();
        var document = new FishOpenSeasonDocument(fishTypes);
        return document.GeneratePdf();
    }

    public async Task<bool> SendFishingLicenseBillAsync(CreateFishingLicenseBillDto createFishingLicenseBillDto,
        string creatorEmail)
    {
        var fishingClub = await _fishingClubRepository.GetFishingClubsAsync();
        foreach (var userId in createFishingLicenseBillDto.UserIds)
        {
            var userWithAddress = await _userRepository.GetUserWithAddressByUserId(userId);
            var fishingLicenseBill = CreateFishingLicenseBill(userWithAddress, fishingClub.FirstOrDefault()!,
                createFishingLicenseBillDto.LicenseYear, DateTime.Now);
            var qrBill = _qrBillService.CreateFishingLicenseBill(fishingLicenseBill);
            var document = new FishingLicenseBillDocument(qrBill, fishingLicenseBill);
            var invoice = document.GeneratePdf();
            await _emailService.SendFishingLicenseBillAsync(createFishingLicenseBillDto.LicenseYear,
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
            await _fishingLicenseRepository.CreateFishingLicenseAsync(createLicenseDto, creatorEmail);
        }

        return true;
    }

    public async Task<byte[]> GetUserFishingLicenseInvoiceAsync(Guid fishingLicenseId)
    {
        var fishingLicense = await _fishingLicenseRepository.GetFishingLicenseAsync(fishingLicenseId);
        var fishingClub = await _fishingClubRepository.GetFishingClubsAsync();
        var userWithAddress = await _userRepository.GetUserWithAddressByUserId(fishingLicense.UserId);
        var fishingLicenseBill = CreateFishingLicenseBill(userWithAddress, fishingClub.FirstOrDefault()!,
            fishingLicense.Year, fishingLicense.CreatedAt);
        var qrBill = _qrBillService.CreateFishingLicenseBill(fishingLicenseBill);
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