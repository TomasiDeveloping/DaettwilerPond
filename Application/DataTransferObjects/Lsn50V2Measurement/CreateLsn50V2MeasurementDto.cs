namespace Application.DataTransferObjects.Lsn50V2Measurement
{
    // Data Transfer Object (DTO) representing the data for creating an LSN50V2 measurement entry
    public class CreateLsn50V2MeasurementDto
    {
        // Sensor identifier for which the measurement entry is created
        public Guid SensorId { get; set; }

        // Temperature value for the measurement entry
        public decimal Temperature { get; set; }

        // Digital status information for the measurement entry
        public string DigitalStatus { get; set; }

        // Indicates whether an external trigger is detected in the measurement entry
        public bool ExtTrigger { get; set; }

        // Indicates whether an open condition is detected in the measurement entry
        public bool Open { get; set; }
    }
}