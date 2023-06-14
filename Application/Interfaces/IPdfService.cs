using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IPdfService
{
    Task<byte[]> CreateMemberPdf();
    Task<byte[]> CreateFishingRulesPdf();
    Task<byte[]> CreateFishOpenSeasonPdf();
    Task<byte[]> CreateFishingLicenseBill();
}