using Newtonsoft.Json;

namespace Application.Models.LSN50v2D20;

public class Lifecycle
{
    [JsonProperty("batteryVoltage")] public double BatteryVoltage { get; set; }

    [JsonProperty("batteryLevel")] public int BatteryLevel { get; set; }
}