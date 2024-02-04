using Application.DataTransferObjects.Lsn50V2Lifecycle;
using Application.DataTransferObjects.Lsn50V2Measurement;
using Application.Interfaces;
using Application.Models.LSN50v2D20;

namespace Persistence.Logic;

// Class providing logic for handling LSN50V2D20 sensor lifecycle and data payload.
public class Lsn50V2D20Logic(ILsn50V2MeasurementRepository lsn50V2MeasurementRepository,
    ILsn50V2LifecycleRepository lsn50V2LifecycleRepository) : ILsn50V2D20Logic
{

    // Method to handle lifecycle data from LSN50V2D20 sensor.
    public async Task HandleLifecycleAsync(Lifecycle lifecycle, Guid sensorId)
    {
        // Create a data transfer object for the new lifecycle data.
        var newLifeCycle = new CreateLsn50V2LifecycleDto
        {
            BatteryLevel = lifecycle.BatteryLevel,
            BatteryVoltage = (decimal) lifecycle.BatteryVoltage,
            SensorId = sensorId
        };

        // Call the repository method to persist the new lifecycle data.
        await lsn50V2LifecycleRepository.CreateLsn50V2LifecycleAsync(newLifeCycle);
    }

    // Method to handle data payload from LSN50V2D20 sensor.
    public async Task HandleDataPayloadAsync(Payload payload, Guid sensorId)
    {
        // Create a data transfer object for the new measurement data.
        var newMeasurement = new CreateLsn50V2MeasurementDto
        {
            SensorId = sensorId,
            DigitalStatus = payload.DigitalStatus,
            ExtTrigger = payload.ExtTrigger,
            Open = payload.Open,
            Temperature = (decimal) payload.Temperature
        };

        // Call the repository method to persist the new measurement data.
        await lsn50V2MeasurementRepository.CreateLsn50V2MeasurementAsync(newMeasurement);
    }
}