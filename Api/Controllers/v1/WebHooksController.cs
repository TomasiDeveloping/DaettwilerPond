using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class WebHooksController : ControllerBase
{
    private readonly ILogger<WebHooksController> _logger;

    public WebHooksController(ILogger<WebHooksController> logger)
    {
        _logger = logger;
    }

    [HttpPost("[action]")]
    public IActionResult TtnWebHook([FromBody] JObject obj)
    {
        try
        {
            _logger.LogInformation(obj.ToString());
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(TtnWebHook)}");
        }
    }
}