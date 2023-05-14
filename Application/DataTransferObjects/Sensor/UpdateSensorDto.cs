namespace Application.DataTransferObjects.Sensor;

public class UpdateSensorDto
{
    public Guid Ïd { get; set; }
    public string DevEui { get; set; }
    public Guid SensorTypeId { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}