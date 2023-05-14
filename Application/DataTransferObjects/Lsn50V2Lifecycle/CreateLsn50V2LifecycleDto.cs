namespace Application.DataTransferObjects.Lsn50V2Lifecycle;

public class CreateLsn50V2LifecycleDto
{
    public Guid SensorId { get; set; }
    public decimal BatteryVoltage { get; set; }
    public int BatteryLevel { get; set; }
}