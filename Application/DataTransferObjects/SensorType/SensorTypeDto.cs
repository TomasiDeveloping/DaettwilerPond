namespace Application.DataTransferObjects.SensorType
{
    // Data Transfer Object (DTO) representing detailed information about a sensor type
    public class SensorTypeDto
    {
        // Unique identifier for the sensor type
        public Guid Id { get; set; }

        // Name of the sensor type
        public string Name { get; set; }

        // Producer or manufacturer of the sensor type
        public string Producer { get; set; }
    }
}