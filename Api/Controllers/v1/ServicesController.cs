using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class ServicesController : ControllerBase
{
    private readonly ILogger<ServicesController> _logger;
    private readonly IPdfService _pdfService;

    public ServicesController(IPdfService pdfService, ILogger<ServicesController> logger)
    {
        _pdfService = pdfService;
        _logger = logger;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetMemberPdf()
    {
        try
        {
            var memberDocument = await _pdfService.CreateMemberPdf();
            return File(memberDocument, "application/pdf", "Mitglieder Fischerclub Dättwiler Weiher.pdf");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetMemberPdf)}");
        }
    }
}