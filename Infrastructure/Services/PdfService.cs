using Application.Interfaces;
using Infrastructure.Documents;
using QuestPDF.Fluent;

namespace Infrastructure.Services;

public class PdfService : IPdfService
{
    private readonly IUserRepository _userRepository;
    private readonly IFishingRegulationRepository _fishingRegulationRepository;

    public PdfService(IUserRepository userRepository, IFishingRegulationRepository fishingRegulationRepository)
    {
        _userRepository = userRepository;
        _fishingRegulationRepository = fishingRegulationRepository;
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
}