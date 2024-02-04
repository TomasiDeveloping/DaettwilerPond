using Application.DataTransferObjects.FishingClub;

namespace Application.Interfaces;

// Interface for managing fishing club-related operations
public interface IFishingClubRepository
{
    // Get a list of all fishing clubs
    Task<List<FishingClubDto>> GetFishingClubsAsync();

    // Get a specific fishing club by its ID
    Task<FishingClubDto> GetFishingClubByIdAsync(Guid fishingClubId);

    // Create a new fishing club
    Task<FishingClubDto> CreateFishingClubAsync(CreateFishingClubDto createFishingClubDto);

    // Update an existing fishing club by its ID
    Task<FishingClubDto> UpdateFishingClubAsync(Guid fishingClubId, FishingClubDto fishingClubDto);

    // Delete a fishing club by its ID
    Task<bool> DeleteFishingClubAsync(Guid fishingClubId);
}