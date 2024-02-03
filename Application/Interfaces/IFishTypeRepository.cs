using Application.DataTransferObjects.FishType;

namespace Application.Interfaces;

// Interface for managing fish type-related operations
public interface IFishTypeRepository
{
    // Get a list of all fish types
    Task<List<FishTypeDto>> GetFishTypesAsync();

    // Create a new fish type with the specified details
    Task<FishTypeDto> CreateFishTypeAsync(CreateFishTypeDto fishTypeDto);

    // Update an existing fish type by its ID
    Task<FishTypeDto> UpdateFishTypeAsync(Guid fishTypeId, FishTypeDto fishTypeDto);

    // Delete a fish type by its ID
    Task<bool> DeleteFishTypeAsync(Guid fishTypeId);
}