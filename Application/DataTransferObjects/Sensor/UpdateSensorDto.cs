namespace Application.DataTransferObjects.Sensor
{
    // Data Transfer Object (DTO) representing the data for updating a sensor
    public class UpdateSensorDto
    {
        // Unique identifier for the sensor
        public Guid Id { get; set; }

        // Unique identifier for the sensor (Device EUI)
        public string DevEui { get; set; }

        // Identifier for the sensor type to which the sensor belongs
        public Guid SensorTypeId { get; set; }

        // Date and time when the sensor was last updated (default to current time)
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}