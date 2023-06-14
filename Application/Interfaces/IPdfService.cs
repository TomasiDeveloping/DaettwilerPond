using System.Threading.Tasks;
using Application.DataTransferObjects.FishingLicense;

namespace Application.Interfaces;

public interface IPdfService
{
    Task<byte[]> CreateMemberPdf();
    Task<byte[]> CreateFishingRulesPdf();
    Task<byte[]> CreateFishOpenSeasonPdf();
    Task<bool> SendFishingLicenseBill(CreateFishingLicenseBillDto createFishingLicenseBillDto);
}