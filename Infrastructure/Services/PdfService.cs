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
    public byte[] CreateMemberPdf()
    {
        var document = new MemberDocument(_userRepository);
        return document.GeneratePdf();
    }
}