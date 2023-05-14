namespace Application.DataTransferObjects.Sensor;

public class CreateSensorDto
{
    public string DevEui { get; set; }
    public Guid SensorTypeId { get; set; }
}