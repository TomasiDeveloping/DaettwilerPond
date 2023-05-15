namespace Application.DataTransferObjects.Lsn50V2Measurement;

public class Lsn50V2TemperatureMeasurementDto
{
    public DateTime ReceivedAt { get; set; }
    public decimal Temperature { get; set; }
}