using Application.DataTransferObjects.FishingRegulation;

namespace Application.Interfaces;

// Interface for managing fishing regulation-related operations
public interface IFishingRegulationRepository
{
    // Get a list of all fishing regulations
    Task<List<FishingRegulationDto>> GetFishingRegulationsAsync();

    // Create a new fishing regulation with the specified details
    Task<FishingRegulationDto> CreateFishingRegulationAsync(CreateFishingRegulationDto createFishingRegulationDto);

    // Update an existing fishing regulation by its ID
    Task<FishingRegulationDto> UpdateFishingRegulationAsync(Guid fishingRegulationId,
        FishingRegulationDto fishingRegulationDto);

    // Delete a fishing regulation by its ID
    Task<bool> DeleteFishingRegulationAsync(Guid fishingRegulationId);
}