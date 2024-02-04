using Application.DataTransferObjects.Lsn50V2Lifecycle;

namespace Application.Interfaces;

// Interface for managing LSN50v2D20 sensor lifecycle-related operations
public interface ILsn50V2LifecycleRepository
{
    // Get a list of all LSN50v2D20 sensor lifecycles
    Task<List<Lsn50V2LifecycleDto>> GetLsn50V2LifecyclesAsync();

    // Get a list of LSN50v2D20 sensor lifecycles associated with a specific sensor ID
    Task<List<Lsn50V2LifecycleDto>> GetLsn50V2LifecyclesBySensorIdAsync(Guid sensorId);

    // Get a specific LSN50v2D20 sensor lifecycle by its ID
    Task<Lsn50V2LifecycleDto> GetLsn50V2LifecycleByIdAsync(Guid id);

    // Create a new LSN50v2D20 sensor lifecycle with the specified details
    Task<Lsn50V2LifecycleDto> CreateLsn50V2LifecycleAsync(CreateLsn50V2LifecycleDto createLsn50V2LifecycleDto);
}