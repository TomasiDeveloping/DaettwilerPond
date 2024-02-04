using Application.Models.LSN50v2D20;

namespace Application.Interfaces;

// Interface for handling LSN50v2D20 sensor logic
public interface ILsn50V2D20Logic
{
    // Handle lifecycle events for the LSN50v2D20 sensor
    Task HandleLifecycleAsync(Lifecycle lifecycle, Guid sensorId);

    // Handle data payload events for the LSN50v2D20 sensor
    Task HandleDataPayloadAsync(Payload payload, Guid sensorId);
}