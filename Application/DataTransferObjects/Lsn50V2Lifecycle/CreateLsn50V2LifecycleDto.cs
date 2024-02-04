namespace Application.DataTransferObjects.Lsn50V2Lifecycle
{
    // Data Transfer Object (DTO) representing the data for creating an LSN50V2 lifecycle entry
    public class CreateLsn50V2LifecycleDto
    {
        // Sensor identifier for which the lifecycle entry is created
        public Guid SensorId { get; set; }

        // Battery voltage value for the lifecycle entry
        public decimal BatteryVoltage { get; set; }

        // Battery level value for the lifecycle entry
        public int BatteryLevel { get; set; }
    }
}