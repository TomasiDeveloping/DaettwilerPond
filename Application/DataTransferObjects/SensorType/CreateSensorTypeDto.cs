namespace Application.DataTransferObjects.SensorType
{
    // Data Transfer Object (DTO) representing the data for creating a sensor type
    public class CreateSensorTypeDto
    {
        // Name of the sensor type
        public string Name { get; set; }

        // Producer or manufacturer of the sensor type
        public string Producer { get; set; }
    }
}