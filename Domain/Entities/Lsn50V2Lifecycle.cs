namespace Domain.Entities
{
    // Entity representing the lifecycle data of an LSN50V2 sensor within the application
    public class Lsn50V2Lifecycle
    {
        // Unique identifier for the lifecycle entry
        public Guid Id { get; set; }

        // The associated sensor for this lifecycle entry
        public Sensor Sensor { get; set; }

        // Unique identifier of the associated sensor
        public Guid SensorId { get; set; }

        // The timestamp when the lifecycle data was received
        public DateTime ReceivedAt { get; set; }

        // The battery voltage of the sensor
        public decimal BatteryVoltage { get; set; }

        // The battery level of the sensor
        public int BatteryLevel { get; set; }
    }
}