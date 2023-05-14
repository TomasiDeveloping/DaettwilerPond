namespace Application.DataTransferObjects.Lsn50V2Measurement;

public class Lsn50V2MeasurementDto
{
    public Guid Id { get; set; }
    public Guid SensorId { get; set; }
    public decimal Temperature { get; set; }
    public string DigitalStatus { get; set; }
    public bool ExtTrigger { get; set; }
    public bool Open { get; set; }
    public DateTime ReceivedAt { get; set; }
}