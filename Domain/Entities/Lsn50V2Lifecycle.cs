namespace Domain.Entities;

public class Lsn50V2Lifecycle
{
    public Guid Id { get; set; }
    public Sensor Sensor { get; set; }
    public Guid SensorId { get; set; }
    public DateTime ReceivedAt { get; set; }
    public double BatteryVoltage { get; set; }
    public int BatteryLevel { get; set; }
}