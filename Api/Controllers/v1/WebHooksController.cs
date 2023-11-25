using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class WebHooksController(IWebHookService webHookService, ILogger<WebHooksController> logger) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> AkenzaWebhook([FromBody] JObject obj)
    {
        try
        {
            var result = await webHookService.AkenzaCallProcessAsync(obj);
            if (!result) logger.LogWarning($"Akenza WebHook was not processed successfully. Received Json: {obj}");
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(AkenzaWebhook)}");
        }
    }
}