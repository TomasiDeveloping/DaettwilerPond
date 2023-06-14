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
    private readonly IFishingRegulationRepository _fishingRegulationRepository;
    private readonly IFishTypeRepository _fishTypeRepository;
    private readonly ISwissQrBillService _qrBillService;
    private readonly IUserRepository _userRepository;

    public PdfService(IUserRepository userRepository, IFishingRegulationRepository fishingRegulationRepository,
        IFishTypeRepository fishTypeRepository, ISwissQrBillService qrBillService,
        IFishingClubRepository fishingClubRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _fishingRegulationRepository = fishingRegulationRepository;
        _fishTypeRepository = fishTypeRepository;
        _qrBillService = qrBillService;
        _fishingClubRepository = fishingClubRepository;
        _emailService = emailService;
    }

    public async Task<byte[]> CreateMemberPdf()
    {
        var userWithAddresses = await _userRepository.GetUsersWithAddressesAsync();
        var document = new MemberDocument(userWithAddresses);
        return document.GeneratePdf();
    }

    public async Task<byte[]> CreateFishingRulesPdf()
    {
        var fishingRules = await _fishingRegulationRepository.GetFishingRegulationsAsync();
        var document = new FishingRulesDocument(fishingRules);
        return document.GeneratePdf();
    }

    public async Task<byte[]> CreateFishOpenSeasonPdf()
    {
        var fishTypes = await _fishTypeRepository.GetFishTypesAsync();
        var document = new FishOpenSeasonDocument(fishTypes);
        return document.GeneratePdf();
    }

    public async Task<bool> SendFishingLicenseBill(CreateFishingLicenseBillDto createFishingLicenseBillDto)
    {
        var fishingClub = await _fishingClubRepository.GetFishingClubsAsync();
        foreach (var userId in createFishingLicenseBillDto.UserIds)
        {
            var userWithAddress = await _userRepository.GetUserWithAddressByUserId(userId);
            var fishingLicenseBill = CreateFishingLicenseBill(userWithAddress, fishingClub.FirstOrDefault()!,
                createFishingLicenseBillDto.LicenseYear);
            var qrBill = _qrBillService.CreateFishingLicenseBill(fishingLicenseBill);
            var document = new FishingLicenseBillDocument(qrBill, fishingLicenseBill);
            var invoice = document.GeneratePdf();
            await _emailService.SendFishingLicenseBill(createFishingLicenseBillDto.LicenseYear,
                    userWithAddress.Email, createFishingLicenseBillDto.EmailMessage, invoice);
        }

        return true;
    }

    private static FishingLicenseBill CreateFishingLicenseBill(UserWithAddressDto userWithAddressDto,
        FishingClubDto fishingClubDto, int licenseYear)
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
            ReferenceMessage = $"Fischerkarte {licenseYear}",
            FishingClubName = fishingClubDto.Name,
            Account = fishingClubDto.IbanNumber
        };
    }
}