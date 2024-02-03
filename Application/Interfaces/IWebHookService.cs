using Newtonsoft.Json.Linq;

namespace Application.Interfaces;

// Interface for handling webhook-related operations
public interface IWebHookService
{
    // Process an Akenza webhook call with the provided JSON object
    Task<bool> AkenzaCallProcessAsync(JObject jObject);
}