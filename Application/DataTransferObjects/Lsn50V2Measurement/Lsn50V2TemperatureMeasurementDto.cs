namespace Application.DataTransferObjects.Lsn50V2Measurement
{
    // Data Transfer Object (DTO) representing temperature information from an LSN50V2 measurement entry
    public class Lsn50V2TemperatureMeasurementDto
    {
        // Date and time when the temperature measurement entry was received
        public DateTime ReceivedAt { get; set; }

        // Temperature value from the measurement entry
        public decimal Temperature { get; set; }
    }
}