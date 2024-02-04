namespace Domain.Entities
{
    // Entity representing the type of sensor within the application
    public class SensorType
    {
        // Unique identifier for the sensor type
        public Guid Id { get; set; }

        // Name of the sensor type
        public string Name { get; set; }

        // Producer of the sensor type
        public string Producer { get; set; }

        // Collection of sensors associated with this sensor type
        public ICollection<Sensor> Sensors { get; set; }
    }
}