using Newtonsoft.Json;

namespace Application.Models.LSN50v2D20;

// Model representing the payload properties of an LSN50v2D20 device
public class Payload
{
    // Temperature reading from the device
    [JsonProperty("temperature")] public double Temperature { get; set; }

    // C0adc value from the device
    [JsonProperty("c0adc")] public int C0adc { get; set; }

    // Digital status information from the device
    [JsonProperty("digitalStatus")] public string DigitalStatus { get; set; }

    // External trigger status from the device
    [JsonProperty("extTrigger")] public bool ExtTrigger { get; set; }

    // Open status from the device
    [JsonProperty("open")] public bool Open { get; set; }

    // C2temperature value from the device
    [JsonProperty("c2temperature")] public double C2temperature { get; set; }

    // C3temperature value from the device
    [JsonProperty("c3temperature")] public double C3temperature { get; set; }
}