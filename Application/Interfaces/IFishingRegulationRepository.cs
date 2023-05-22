using Application.DataTransferObjects.FishingRegulation;

namespace Application.Interfaces;

public interface IFishingRegulationRepository
{
    Task<List<FishingRegulationDto>> GetFishingRegulationsAsync();
    Task<FishingRegulationDto> CreateFishingRegulationAsync(FishingRegulationDto fishingRegulationDto);

    Task<FishingRegulationDto> UpdateFishingRegulationAsync(Guid fishingRegulationId,
        FishingRegulationDto fishingRegulationDto);

    Task<bool> DeleteFishingRegulationAsync(Guid fishingRegulationId);
}