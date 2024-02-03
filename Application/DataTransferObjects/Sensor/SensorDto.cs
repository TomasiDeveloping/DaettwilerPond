namespace Application.DataTransferObjects.Sensor
{
    // Data Transfer Object (DTO) representing detailed information about a sensor
    public class SensorDto
    {
        // Unique identifier for the sensor
        public Guid Id { get; set; }

        // Unique identifier for the sensor (Device EUI)
        public string DevEui { get; set; }

        // Name of the sensor type to which the sensor belongs
        public string SensorTypeName { get; set; }

        // Producer of the sensor type to which the sensor belongs
        public string SensorTypeProducer { get; set; }

        // Identifier for the sensor type to which the sensor belongs
        public Guid SensorTypeId { get; set; }

        // Date and time when the sensor was created
        public DateTime CreatedAt { get; set; }

        // Date and time when the sensor was last updated (nullable)
        public DateTime? UpdatedAt { get; set; }
    }
}