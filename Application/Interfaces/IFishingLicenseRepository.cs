using Application.DataTransferObjects.FishingLicense;
using Application.DataTransferObjects.Overseer;
using Application.Models.CatchReport;

namespace Application.Interfaces;

// Interface for managing fishing license-related operations
public interface IFishingLicenseRepository
{
    // Get a list of all fishing licenses
    Task<List<FishingLicenseDto>> GetFishingLicensesAsync();

    // Get a specific fishing license by its ID
    Task<FishingLicenseDto> GetFishingLicenseAsync(Guid fishingLicenseId);

    // Get a list of fishing licenses associated with a specific user
    Task<List<FishingLicenseDto>> GetUserFishingLicenses(Guid userId);

    // Get the fishing license for the current year for a specific user
    Task<FishingLicenseDto> GetUserFishingLicenseForCurrentYear(Guid userId);

    Task<CatchDetailsYearDto> GetDetailYearlyCatchAsync(int year);

    Task<List<UserStatistic>> GetDetailYearlyCatchReportAsync(int year);

    Task<UserStatistic> GetYearlyUserCatchReportAsync(Guid userId, int year);

    Task<OverseerMemberDetailsDto> GetOverseerMemberDetailAsync(Guid userId);

    // Create a new fishing license with the specified details and issuer
    Task<FishingLicenseDto> CreateFishingLicenseAsync(CreateFishingLicenseDto createFishingLicenseDto,
        string issuer);

    // Update an existing fishing license
    Task<FishingLicenseDto> UpdateFishingLicenseAsync(FishingLicenseDto fishingLicenseDto);

    // Delete a fishing license by its ID
    Task<bool> DeleteFishingLicenseAsync(Guid fishingLicenseId);
}