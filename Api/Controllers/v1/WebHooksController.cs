using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class WebHooksController : ControllerBase
{
    private readonly IWebHookService _webHookService;
    private readonly ILogger<WebHooksController> _logger;

    public WebHooksController(IWebHookService webHookService, ILogger<WebHooksController> logger)
    {
        _webHookService = webHookService;
        _logger = logger;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AkenzaWebhook([FromBody] JObject obj)
    {
        try
        {
            var result = await _webHookService.AkenzaCallProcessAsync(obj);
            if (!result) _logger.LogWarning($"Akenza WebHook was not processed successfully. Received Json: {obj}");
                return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(AkenzaWebhook)}");
        }
    }
}