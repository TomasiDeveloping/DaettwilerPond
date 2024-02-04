using Application.Interfaces;
using Application.Models.LSN50v2D20;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services;

public class WebHookService(ILsn50V2D20Logic lsn50V2D20Logic, ILogger<WebHookService> logger,
    ISensorRepository sensorRepository) : IWebHookService
{
    public async Task<bool> AkenzaCallProcessAsync(JObject jObject)
    {
        // Extracting topic and deviceId (devEui) from the received JSON
        var topic = jObject.SelectToken("topic")?.ToString();
        var devEui = jObject.SelectToken("deviceId")?.ToString();

        // Check if deviceId (devEui) is present in the JSON
        if (devEui == null)
        {
            logger.LogError("No DevEui found in Json");
            return false;
        }

        // Retrieve sensor information based on deviceId (devEui)
        var sensor = await sensorRepository.GetSensorByDevEuiAsync(devEui);

        // Log a warning if no sensor is found for the specified deviceId (devEui)
        if (sensor == null) logger.LogError($"No Sensor found with devEui: {devEui}");

        // Handle different topics (lifecycle, default, ds)
        switch (topic)
        {
            case "lifecycle":
                // Deserialize JSON data for the lifecycle topic and handle it
                var lifecycle = JsonConvert.DeserializeObject<Lifecycle>(jObject.SelectToken("data")?.ToString() ?? throw new InvalidOperationException());

                // Handle the lifecycle data, associating it with the sensor if available
                if (sensor is not null) await lsn50V2D20Logic.HandleLifecycleAsync(lifecycle, sensor.Id);
                break;
            case "default":
                // Deserialize JSON data for the default topic and handle it
                var payload = JsonConvert.DeserializeObject<Payload>(jObject.SelectToken("data")?.ToString() ??
                                                                     throw new InvalidOperationException());

                // Handle the default payload data, associating it with the sensor if available
                if (sensor is not null) await lsn50V2D20Logic.HandleDataPayloadAsync(payload, sensor.Id);
                break;
            case "ds":
                // Log information for the 'ds' payload
                logger.LogInformation("ds Payload received");
                break;
            default:
                // Log a warning for unsupported topics
                logger.LogWarning($"Unsupported topic: {topic}");
                break;
        }

        return true;
    }
}