namespace Application.DataTransferObjects.Lsn50V2Measurement;

public class TemperatureHistoryDto
{
    public decimal MaximumTemperatureMonth { get; set; }
    public decimal MinimumTemperatureMonth { get; set; }
    public decimal TemperatureAverageMonth { get; set; }
    public decimal MaximumTemperatureYear { get; set; }
    public decimal MinimumTemperatureYear { get; set; }
    public decimal TemperatureAverageYear { get; set; }
    public decimal MaximumTemperature { get; set; }
    public DateTime MaximumTemperatureReceivedTime { get; set; }
    public decimal MinimumTemperature { get; set; }
    public DateTime MinimumTemperatureReceivedTime { get; set; }
}