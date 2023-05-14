using Application.Interfaces;
using Application.Models.LSN50v2D20;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services;

public class WebHookService : IWebHookService
{
    private readonly ILsn50V2d20Logic _lsn50V2d20Logic;
    private readonly ILogger<WebHookService> _logger;

    public WebHookService(ILsn50V2d20Logic lsn50V2d20Logic, ILogger<WebHookService> logger)
    {
        _lsn50V2d20Logic = lsn50V2d20Logic;
        _logger = logger;
    }
    public async Task<bool> AkenzaCallProcessAsync(JObject jObject)
    {
        var topic = jObject.SelectToken("topic")?.ToString();
        var devEui = jObject.SelectToken("deviceId")?.ToString();
        switch (topic)
        {
            case "lifecycle":
                var lifecycle = JsonConvert.DeserializeObject<Lifecycle>(jObject.SelectToken("data")?.ToString() ?? throw new InvalidOperationException());
                await _lsn50V2d20Logic.HandleLifecycleAsync(lifecycle, devEui);
                break;
            case "ds":
                var payload = JsonConvert.DeserializeObject<Payload>(jObject.SelectToken("data")?.ToString() ?? throw new InvalidOperationException());
                await _lsn50V2d20Logic.HandleDataPayloadAsync(payload, devEui);
                break;
            default:
                _logger.LogWarning($"Unsupported topic: {topic}");
                break;
        }

        return true;
    }
}