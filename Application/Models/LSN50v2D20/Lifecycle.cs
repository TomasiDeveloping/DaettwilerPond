using Newtonsoft.Json;

namespace Application.Models.LSN50v2D20;

// Model representing the lifecycle properties of an LSN50v2D20 device
public class Lifecycle
{
    // Battery voltage of the device
    [JsonProperty("batteryVoltage")] public double BatteryVoltage { get; set; }

    // Battery level of the device
    [JsonProperty("batteryLevel")] public int BatteryLevel { get; set; }
}