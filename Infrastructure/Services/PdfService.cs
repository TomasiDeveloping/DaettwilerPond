using Application.Interfaces;
using Infrastructure.Documents;
using QuestPDF.Fluent;

namespace Infrastructure.Services;

public class PdfService : IPdfService
{
    private readonly IUserRepository _userRepository;
    private readonly IFishingRegulationRepository _fishingRegulationRepository;
    private readonly IFishTypeRepository _fishTypeRepository;
    private readonly ISwissQrBillService _qrBillService;

    public PdfService(IUserRepository userRepository, IFishingRegulationRepository fishingRegulationRepository, IFishTypeRepository fishTypeRepository, ISwissQrBillService qrBillService)
    {
        _userRepository = userRepository;
        _fishingRegulationRepository = fishingRegulationRepository;
        _fishTypeRepository = fishTypeRepository;
        _qrBillService = qrBillService;
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

    public async Task<byte[]> CreateFishingLicenseBill()
    {
        var userWithAddress = await _userRepository.GetUsersWithAddressesAsync();
        var qrBill =  _qrBillService.CreateFishingLicenseBill();
        var document = new FishingLicenseBillDocument(qrBill);
        return document.GeneratePdf();
    }
}