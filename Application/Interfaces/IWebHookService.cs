using Newtonsoft.Json.Linq;

namespace Application.Interfaces;

public interface IWebHookService
{
    Task<bool> AkenzaCallProcessAsync(JObject jObject);
}