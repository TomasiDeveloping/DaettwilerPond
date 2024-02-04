namespace Domain.Entities
{
    // Entity representing the measurement data of an LSN50V2 sensor within the application
    public class Lsn50V2Measurement
    {
        // Unique identifier for the measurement entry
        public Guid Id { get; set; }

        // The associated sensor for this measurement entry
        public Sensor Sensor { get; set; }

        // Unique identifier of the associated sensor
        public Guid SensorId { get; set; }

        // The temperature measured by the sensor
        public decimal Temperature { get; set; }

        // The digital status information from the sensor
        public string DigitalStatus { get; set; }

        // Indicates whether there was an external trigger
        public bool ExtTrigger { get; set; }

        // Indicates whether the sensor is open
        public bool Open { get; set; }

        // The timestamp when the measurement data was received
        public DateTime ReceivedAt { get; set; }
    }
}