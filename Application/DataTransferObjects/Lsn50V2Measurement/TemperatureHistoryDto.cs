namespace Application.DataTransferObjects.Lsn50V2Measurement
{
    // Data Transfer Object (DTO) representing temperature history metrics from LSN50V2 measurements
    public class TemperatureHistoryDto
    {
        // Maximum temperature for the current month
        public decimal MaximumTemperatureMonth { get; set; }

        // Minimum temperature for the current month
        public decimal MinimumTemperatureMonth { get; set; }

        // Average temperature for the current month
        public decimal TemperatureAverageMonth { get; set; }

        // Maximum temperature for the current year
        public decimal MaximumTemperatureYear { get; set; }

        // Minimum temperature for the current year
        public decimal MinimumTemperatureYear { get; set; }

        // Average temperature for the current year
        public decimal TemperatureAverageYear { get; set; }

        // Overall maximum temperature
        public decimal MaximumTemperature { get; set; }

        // Date and time when the maximum temperature was received
        public DateTime MaximumTemperatureReceivedTime { get; set; }

        // Overall minimum temperature
        public decimal MinimumTemperature { get; set; }

        // Date and time when the minimum temperature was received
        public DateTime MinimumTemperatureReceivedTime { get; set; }
    }
}