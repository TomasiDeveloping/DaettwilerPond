namespace Application.DataTransferObjects.Sensor
{
    // Data Transfer Object (DTO) representing the data for creating a sensor
    public class CreateSensorDto
    {
        // Unique identifier for the sensor (Device EUI)
        public string DevEui { get; set; }

        // Identifier for the sensor type to which the sensor belongs
        public Guid SensorTypeId { get; set; }
    }
}