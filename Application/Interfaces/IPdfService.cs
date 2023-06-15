using Application.DataTransferObjects.FishingLicense;

namespace Application.Interfaces;

public interface IPdfService
{
    Task<byte[]> CreateMemberPdfAsync();
    Task<byte[]> CreateFishingRulesPdfAsync();
    Task<byte[]> CreateFishOpenSeasonPdfAsync();
    Task<bool> SendFishingLicenseBillAsync(CreateFishingLicenseBillDto createFishingLicenseBillDto, string creatorEmail);
    Task<byte[]> GetUserFishingLicenseInvoiceAsync(Guid fishingLicenseId);
}