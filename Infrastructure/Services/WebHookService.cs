using Application.Interfaces;
using Application.Models.LSN50v2D20;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services;

public class WebHookService : IWebHookService
{
    private readonly ILogger<WebHookService> _logger;
    private readonly ILsn50V2d20Logic _lsn50V2d20Logic;
    private readonly ISensorRepository _sensorRepository;

    public WebHookService(ILsn50V2d20Logic lsn50V2d20Logic, ILogger<WebHookService> logger,
        ISensorRepository sensorRepository)
    {
        _lsn50V2d20Logic = lsn50V2d20Logic;
        _logger = logger;
        _sensorRepository = sensorRepository;
    }

    public async Task<bool> AkenzaCallProcessAsync(JObject jObject)
    {
        var topic = jObject.SelectToken("topic")?.ToString();
        var devEui = jObject.SelectToken("deviceId")?.ToString();
        if (devEui == null)
        {
            _logger.LogError("No DevEui found in Json");
            return false;
        }

        var sensor = await _sensorRepository.GetSensorByDevEuiAsync(devEui);
        if (sensor == null) _logger.LogError($"No Sensor found with devEui: {devEui}");
        switch (topic)
        {
            case "lifecycle":
                var lifecycle = JsonConvert.DeserializeObject<Lifecycle>(jObject.SelectToken("data")?.ToString() ??
                                                                         throw new InvalidOperationException());
                await _lsn50V2d20Logic.HandleLifecycleAsync(lifecycle, sensor.Id);
                break;
            case "default":
                var payload = JsonConvert.DeserializeObject<Payload>(jObject.SelectToken("data")?.ToString() ??
                                                                     throw new InvalidOperationException());
                await _lsn50V2d20Logic.HandleDataPayloadAsync(payload, sensor.Id);
                break;
            case "ds":
                _logger.LogInformation("ds Payload received");
                break;
            default:
                _logger.LogWarning($"Unsupported topic: {topic}");
                break;
        }

        return true;
    }
}