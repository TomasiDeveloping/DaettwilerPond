using Application.DataTransferObjects.CatchDetail;

namespace Application.Interfaces;

// Interface for managing catch detail-related operations
public interface ICatchDetailRepository
{
    // Get a specific catch detail by its ID
    Task<CatchDetailDto> GetCatchDetailAsync(Guid catchDetailId);

    // Get a list of catch details associated with a specific catch ID
    Task<List<CatchDetailDto>> GetCatchDetailsByCatchId(Guid catchId);

    // Create a new catch detail
    Task<CatchDetailDto> CreateCatchDetailAsync(CreateCatchDetailDto createCatchDetailDto);

    // Update an existing catch detail
    Task<CatchDetailDto> UpdateCatchDetailAsync(UpdateCatchDetailDto updateCatchDetailDto);

    // Delete a catch detail by its ID
    Task<bool> DeleteCatchDetailAsync(Guid catchDetailId);
}