namespace Application.DataTransferObjects.Lsn50V2Lifecycle
{
    // Data Transfer Object (DTO) representing detailed information about an LSN50V2 lifecycle entry
    public class Lsn50V2LifecycleDto
    {
        // Unique identifier for the LSN50V2 lifecycle entry
        public Guid Id { get; set; }

        // Sensor identifier for which the lifecycle entry belongs
        public Guid SensorId { get; set; }

        // Date and time when the lifecycle entry was received
        public DateTime ReceivedAt { get; set; }

        // Battery voltage value for the lifecycle entry
        public decimal BatteryVoltage { get; set; }

        // Battery level value for the lifecycle entry
        public int BatteryLevel { get; set; }
    }
}