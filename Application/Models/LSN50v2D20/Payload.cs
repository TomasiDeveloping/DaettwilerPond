using Newtonsoft.Json;

namespace Application.Models.LSN50v2D20;

public class Payload
{
    [JsonProperty("temperature")] public double Temperature { get; set; }

    [JsonProperty("c0adc")] public int C0adc { get; set; }

    [JsonProperty("digitalStatus")] public string DigitalStatus { get; set; }

    [JsonProperty("extTrigger")] public bool ExtTrigger { get; set; }

    [JsonProperty("open")] public bool Open { get; set; }

    [JsonProperty("c2temperature")] public double C2temperature { get; set; }

    [JsonProperty("c3temperature")] public double C3temperature { get; set; }
}