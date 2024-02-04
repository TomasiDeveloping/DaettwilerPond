namespace Application.DataTransferObjects.Lsn50V2Measurement
{
    // Data Transfer Object (DTO) representing detailed information about an LSN50V2 measurement entry
    public class Lsn50V2MeasurementDto
    {
        // Unique identifier for the LSN50V2 measurement entry
        public Guid Id { get; set; }

        // Sensor identifier for which the measurement entry belongs
        public Guid SensorId { get; set; }

        // Temperature value for the measurement entry
        public decimal Temperature { get; set; }

        // Digital status information for the measurement entry
        public string DigitalStatus { get; set; }

        // Indicates whether an external trigger is detected in the measurement entry
        public bool ExtTrigger { get; set; }

        // Indicates whether an open condition is detected in the measurement entry
        public bool Open { get; set; }

        // Date and time when the measurement entry was received
        public DateTime ReceivedAt { get; set; }
    }
}