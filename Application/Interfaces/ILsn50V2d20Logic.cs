using Application.Models.LSN50v2D20;

namespace Application.Interfaces;

public interface ILsn50V2d20Logic
{
    Task HandleLifecycleAsync(Lifecycle lifecycle, Guid sensorId);
    Task HandleDataPayloadAsync(Payload payload, Guid sensorId);
}