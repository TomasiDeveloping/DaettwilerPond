using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Api.Controllers.v1;

// Define the route for the controller with versioning support
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class WebHooksController(IWebHookService webHookService, ILogger<WebHooksController> logger) : ControllerBase
{

    // Handle incoming Akenza WebHook requests
    [HttpPost("[action]")]
    public async Task<IActionResult> AkenzaWebhook([FromBody] JObject obj)
    {
        try
        {
            // Call the Akenza WebHook processing service
            var result = await webHookService.AkenzaCallProcessAsync(obj);

            // Log a warning if the Akenza WebHook was not processed successfully
            if (!result) logger.LogWarning($"Akenza WebHook was not processed successfully. Received Json: {obj}");

            // Return a 200 OK response
            return Ok();
        }
        catch (Exception e)
        {
            // Log error and return a 500 Internal Server Error
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(AkenzaWebhook)}");
        }
    }
}