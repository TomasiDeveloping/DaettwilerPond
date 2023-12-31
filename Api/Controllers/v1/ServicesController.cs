using System.Security.Claims;
using Application.DataTransferObjects.FishingLicense;
using Application.DataTransferObjects.Services;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class ServicesController(IPdfService pdfService, ILogger<ServicesController> logger, IEmailService emailService) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<IActionResult> GetMemberPdf()
    {
        try
        {
            var memberDocument = await pdfService.CreateMemberPdfAsync();
            const string fileName = "Mitglieder_Fischerclub_Daettwiler_Weiher.pdf";
            Response.Headers.Append("x-file-name", fileName);
            Response.Headers.Append("Access-Control-Expose-Headers", "x-file-name");
            return File(memberDocument, "application/pdf", fileName);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetMemberPdf)}");
        }
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetFishingRulesPdf()
    {
        try
        {
            var fishingRuleDocument = await pdfService.CreateFishingRulesPdfAsync();
            const string fileName = "Vorschriften.pdf";
            Response.Headers.Append("x-file-name", fileName);
            Response.Headers.Append("Access-Control-Expose-Headers", "x-file-name");
            return File(fishingRuleDocument, "application/pdf", fileName);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingRulesPdf)}");
        }
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetFishOpenSeasonPdf()
    {
        try
        {
            var fishOpenSeasonDocument = await pdfService.CreateFishOpenSeasonPdfAsync();
            const string fileName = "Schonmass_und_Schonzeiten.pdf";
            Response.Headers.Append("x-file-name", fileName);
            Response.Headers.Append("Access-Control-Expose-Headers", "x-file-name");
            return File(fishOpenSeasonDocument, "application/pdf", fileName);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishOpenSeasonPdf)}");
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<bool>> SendFishingLicenseInvoice(
        CreateFishingLicenseBillDto createFishingLicenseBillDto)
    {
        try
        {
            var response = await pdfService.SendFishingLicenseBillAsync(createFishingLicenseBillDto, GetUserEmail());
            return response ? Ok(true) : BadRequest("Error in send QRBill");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(SendFishingLicenseInvoice)}");
        }
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<ActionResult<bool>> SendMembersEmail([FromForm] MembersEmailDto membersEmailDto)
    {
        try
        {
            var response = await emailService.SendEmailToMembersAsync(membersEmailDto.ReceiverAddresses,
                membersEmailDto.Subject, membersEmailDto.MailContent, membersEmailDto.Attachments);
            return response ? Ok(true) : BadRequest("Error send email to members");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(SendMembersEmail)}");
        }
    }

    [HttpGet("[action]/{fishingLicenseId}")]
    public async Task<IActionResult> GetUserInvoiceFishingLicense(Guid fishingLicenseId)
    {
        try
        {
            var fishingLicenseInvoice = await pdfService.GetUserFishingLicenseInvoiceAsync(fishingLicenseId);
            const string fileName = "Rechnung_Fischerkarte.pdf";
            Response.Headers.Append("x-file-name", fileName);
            Response.Headers.Append("Access-Control-Expose-Headers", "x-file-name");
            return File(fishingLicenseInvoice, "application/pdf", fileName);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUserInvoiceFishingLicense)}");
        }
    }

    private string GetUserEmail()
    {
        var email = HttpContext.User.Identity is ClaimsIdentity identity
            ? identity.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown"
            : "Unknown";
        return email;
    }
}