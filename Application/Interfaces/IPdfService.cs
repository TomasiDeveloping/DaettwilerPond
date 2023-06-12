namespace Application.Interfaces;

public interface IPdfService
{
    Task<byte[]> CreateMemberPdf();
    Task<byte[]> CreateFishingRulesPdf();
}