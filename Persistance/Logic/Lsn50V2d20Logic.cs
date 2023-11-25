using Application.DataTransferObjects.Lsn50V2Lifecycle;
using Application.DataTransferObjects.Lsn50V2Measurement;
using Application.Interfaces;
using Application.Models.LSN50v2D20;

namespace Persistence.Logic;

public class Lsn50V2d20Logic(ILsn50V2MeasurementRepository lsn50V2MeasurementRepository,
    ILsn50V2LifecycleRepository lsn50V2LifecycleRepository) : ILsn50V2d20Logic
{
    public async Task HandleLifecycleAsync(Lifecycle lifecycle, Guid sensorId)
    {
        var newLifeCycle = new CreateLsn50V2LifecycleDto
        {
            BatteryLevel = lifecycle.BatteryLevel,
            BatteryVoltage = (decimal) lifecycle.BatteryVoltage,
            SensorId = sensorId
        };
        await lsn50V2LifecycleRepository.CreateLsn50V2LifecycleAsync(newLifeCycle);
    }

    public async Task HandleDataPayloadAsync(Payload payload, Guid sensorId)
    {
        var newMeasurement = new CreateLsn50V2MeasurementDto
        {
            SensorId = sensorId,
            DigitalStatus = payload.DigitalStatus,
            ExtTrigger = payload.ExtTrigger,
            Open = payload.Open,
            Temperature = (decimal) payload.Temperature
        };
        await lsn50V2MeasurementRepository.CreateLsn50V2MeasurementAsync(newMeasurement);
    }
}