using Application.DataTransferObjects.Catch;

namespace Application.Interfaces;

// Interface for managing catch-related operations
public interface ICatchRepository
{
    // Get a list of catches associated with a specific licence ID
    Task<List<CatchDto>> GetCatchesByLicenceIdAsync(Guid licenceId);

    // Check if a catch with the given date already exists for a specific licence
    Task<bool> CheckCatchDateExistsAsync(Guid licenceId, DateTime catchDate);

    // Get a specific catch by its ID
    Task<CatchDto> GetCatchAsync(Guid catchId);

    // Get a list of catches for a specific month and licence ID
    Task<List<CatchDto>> GetCatchesForMonthAsync(Guid licenceId, int month);

    // Get the catch for the current day and licence ID
    Task<CatchDto> GetCatchForCurrentDayAsync(Guid licenceId);

    // Get the yearly catch summary for a specific licence ID
    Task<YearlyCatch> GetYearlyCatchAsync(Guid licenceId);

    // Get detailed yearly catch information for a specific licence ID
    Task<List<DetailYearlyCatch>> GetDetailYearlyCatchAsync(Guid licenceId);

    // Update an existing catch
    Task<CatchDto> UpdateCatchAsync(UpdateCatchDto updateCatchDto);

    // Create a new catch
    Task<CatchDto> CreateCatchAsync(CreateCatchDto createCatchDto);

    // Start a new fishing day for the given licence ID
    Task<CatchDto> StartCatchDayAsync(Guid licenceId);

    // Stop the current fishing day for the given catch ID
    Task<CatchDto> StopCatchDayAsync(Guid catchId);

    // Continue the fishing day for the given catch ID
    Task<CatchDto> ContinueFishingDayAsync(Guid catchId);

    // Delete a catch by its ID
    Task<bool> DeleteCatchAsync(Guid catchId);
}