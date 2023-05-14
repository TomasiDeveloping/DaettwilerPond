namespace Domain.Entities;

public class Sensor
{
    public Guid Id { get; set; }
    public string DevEui { get; set; }
    public SensorType SensorType { get; set; }
    public Guid SensorTypeId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}