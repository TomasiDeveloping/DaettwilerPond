using Application.Interfaces;
using Infrastructure.Documents;
using QuestPDF.Fluent;

namespace Infrastructure.Services;

public class PdfService : IPdfService
{
    private readonly IUserRepository _userRepository;

    public PdfService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<byte[]> CreateMemberPdf()
    {
        var userWithAddresses = await _userRepository.GetUsersWithAddressesAsync();
        var document = new MemberDocument(userWithAddresses);
        return document.GeneratePdf();
    }
}