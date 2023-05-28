using Application.DataTransferObjects.FishingLicense;

namespace Application.Interfaces;

public interface IFishingLicenseRepository
{
    Task<List<FishingLicenseDto>> GetFishingLicensesAsync();
    Task<FishingLicenseDto> GetFishingLicenseAsync(Guid fishingLicenseId);
    Task<List<FishingLicenseDto>> GetUserFishingLicenses(Guid userId);
    Task<FishingLicenseDto> GetUserFishingLicenseForCurrentYear(Guid userId);

    Task<FishingLicenseDto> CreateFishingLicenseAsync(CreateFishingLicenseDto createFishingLicenseDto,
        string issuer);

    Task<FishingLicenseDto> UpdateFishingLicenseAsync(FishingLicenseDto fishingLicenseDto);
    Task<bool> DeleteFishingLicenseAsync(Guid fishingLicenseId);
}