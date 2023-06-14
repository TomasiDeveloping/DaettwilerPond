using Application.DataTransferObjects.FishingClub;

namespace Application.Interfaces;

public interface IFishingClubRepository
{
    Task<List<FishingClubDto>> GetFishingClubsAsync();
    Task<FishingClubDto> GetFishingClubByIdAsync(Guid fishingClubId);
    Task<FishingClubDto> CreateFishingClubAsync(CreateFishingClubDto createFishingClubDto);
    Task<FishingClubDto> UpdateFishingClubAsync(Guid fishingClubId, FishingClubDto fishingClubDto);
    Task<bool> DeleteFishingClubAsync(Guid fishingClubId);
}