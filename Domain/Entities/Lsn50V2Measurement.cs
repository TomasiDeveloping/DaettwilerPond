namespace Domain.Entities;

public class Lsn50V2Measurement
{
    public Guid Id { get; set; }
    public Sensor Sensor { get; set; }
    public Guid SensorId { get; set; }
    public decimal Temperature { get; set; }
    public string DigitalStatus { get; set; }
    public bool ExtTrigger { get; set; }
    public bool Open { get; set; }
    public DateTime ReceivedAt { get; set; }
}