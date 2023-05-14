namespace Application.DataTransferObjects.Lsn50V2Lifecycle;

public class Lsn50V2LifecycleDto
{
    public Guid Id { get; set; }
    public Guid SensorId { get; set; }
    public DateTime ReceivedAt { get; set; }
    public double BatteryVoltage { get; set; }
    public int BatteryLevel { get; set; }
}