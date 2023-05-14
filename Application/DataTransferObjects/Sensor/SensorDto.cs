namespace Application.DataTransferObjects.Sensor;

public class SensorDto
{
    public Guid Id { get; set; }
    public string DevEui { get; set; }
    public string SensorTypeName { get; set; }
    public string SensorTypeProducer { get; set; }
    public Guid SensorTypeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}