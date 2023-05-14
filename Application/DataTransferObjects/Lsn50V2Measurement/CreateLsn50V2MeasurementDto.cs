namespace Application.DataTransferObjects.Lsn50V2Measurement;

public class CreateLsn50V2MeasurementDto
{
    public Guid SensorId { get; set; }
    public decimal Temperature { get; set; }
    public string DigitalStatus { get; set; }
    public bool ExtTrigger { get; set; }
    public bool Open { get; set; }
}