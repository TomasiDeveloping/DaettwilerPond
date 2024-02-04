namespace Domain.Entities
{
    // Entity representing a sensor within the application
    public class Sensor
    {
        // Unique identifier for the sensor
        public Guid Id { get; set; }

        // Device EUI (Extended Unique Identifier) of the sensor
        public string DevEui { get; set; }

        // The type of sensor
        public SensorType SensorType { get; set; }

        // Unique identifier of the associated sensor type
        public Guid SensorTypeId { get; set; }

        // The timestamp when the sensor was created
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // The timestamp when the sensor was last updated, nullable
        public DateTime? UpdatedAt { get; set; }
    }
}