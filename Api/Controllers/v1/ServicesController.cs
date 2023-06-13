﻿using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
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
            const string fileName = "Mitglieder_Fischerclub_Daettwiler_Weiher.pdf";
            Response.Headers.Add("x-file-name", fileName);
            Response.Headers.Add("Access-Control-Expose-Headers", "x-file-name");
            return File(memberDocument, "application/pdf", fileName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetMemberPdf)}");
        }
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetFishingRulesPdf()
    {
        try
        {
            var fishingRuleDocument = await _pdfService.CreateFishingRulesPdf();
            const string fileName = "Vorschriften.pdf";
            Response.Headers.Add("x-file-name", fileName);
            Response.Headers.Add("Access-Control-Expose-Headers", "x-file-name");
            return File(fishingRuleDocument, "application/pdf", fileName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingRulesPdf)}");
        }
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetFishOpenSeasonPdf()
    {
        try
        {
            var fishOpenSeasonDocument = await _pdfService.CreateFishOpenSeasonPdf();
            const string fileName = "Schonmass_und_Schonzeiten.pdf";
            Response.Headers.Add("x-file-name", fileName);
            Response.Headers.Add("Access-Control-Expose-Headers", "x-file-name");
            return File(fishOpenSeasonDocument, "application/pdf", fileName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishOpenSeasonPdf)}");
        }
    }
}