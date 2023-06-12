namespace Application.Interfaces;

public interface IPdfService
{
    Task<byte[]> CreateMemberPdf();
}