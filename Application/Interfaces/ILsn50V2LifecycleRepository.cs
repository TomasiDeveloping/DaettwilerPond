using Application.DataTransferObjects.Lsn50V2Lifecycle;

namespace Application.Interfaces;

public interface ILsn50V2LifecycleRepository
{
    Task<List<Lsn50V2LifecycleDto>> GetLsn50V2LifecyclesAsync();
    Task<List<Lsn50V2LifecycleDto>> GetLsn50V2LifecyclesBySensorIdAsync(Guid sensorId);
    Task<Lsn50V2LifecycleDto> GetLsn50V2LifecycleByIdAsync(Guid id);
    Task<Lsn50V2LifecycleDto> CreateLsn50V2LifecycleAsync(CreateLsn50V2LifecycleDto createLsn50V2LifecycleDto);
}