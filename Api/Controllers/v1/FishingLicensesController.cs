using System.Security.Claims;
using Application.DataTransferObjects.FishingLicense;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class FishingLicensesController(IFishingLicenseRepository fishingLicenseRepository,
    ILogger<FishingLicensesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<FishingLicenseDto>>> GetFishingLicenses()
    {
        try
        {
            var licenses = await fishingLicenseRepository.GetFishingLicensesAsync();
            return licenses.Any() ? Ok(licenses) : NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingLicenses)}");
        }
    }

    [HttpGet("{fishingLicenseId}")]
    public async Task<ActionResult<FishingLicenseDto>> GetFishingLicense(Guid fishingLicenseId)
    {
        try
        {
            var license = await fishingLicenseRepository.GetFishingLicenseAsync(fishingLicenseId);
            if (license == null) return BadRequest($"Keine Lizenz mit der Id: {fishingLicenseId} gefunden");
            return Ok(license);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingLicense)}");
        }
    }

    [HttpGet("Users/{userId}")]
    public async Task<ActionResult<List<FishingLicenseDto>>> GetUserFishingLicenses(Guid userId)
    {
        try
        {
            var userLicenses = await fishingLicenseRepository.GetUserFishingLicenses(userId);
            return userLicenses.Any() ? Ok(userLicenses) : NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUserFishingLicenses)}");
        }
    }

    [HttpGet("Users/[action]/{userId}")]
    public async Task<ActionResult<FishingLicenseDto>> GetCurrentUserLicense(Guid userId)
    {
        try
        {
            var currentUserLicense = await fishingLicenseRepository.GetUserFishingLicenseForCurrentYear(userId);
            if (currentUserLicense == null) return BadRequest("Keine Aktuelle Lizenz vorhanden");
            return Ok(currentUserLicense);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetCurrentUserLicense)}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<FishingLicenseDto>> CreateFishingLicense(
        CreateFishingLicenseDto createFishingLicenseDto)
    {
        try
        {
            var newLicense =
                await fishingLicenseRepository.CreateFishingLicenseAsync(createFishingLicenseDto, GetUserEmail());
            if (newLicense == null) return BadRequest("Lizenz konnte nicht erstellt werden");
            return Ok(newLicense);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateFishingLicense)}");
        }
    }

    [HttpPut("{fishingLicenseId}")]
    public async Task<ActionResult<FishingLicenseDto>> UpdateFishingLicense(Guid fishingLicenseId,
        FishingLicenseDto fishingLicenseDto)
    {
        try
        {
            if (fishingLicenseId != fishingLicenseDto.Id) return BadRequest("Fehler in Id's");
            var updatedLicense = await fishingLicenseRepository.UpdateFishingLicenseAsync(fishingLicenseDto);
            if (updatedLicense == null) return BadRequest("Lizenz konnte nicht geupdated werden");
            return Ok(updatedLicense);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateFishingLicense)}");
        }
    }

    [HttpDelete("{fishingLicenseId}")]
    public async Task<ActionResult<bool>> DeleteFishingLicense(Guid fishingLicenseId)
    {
        try
        {
            var result = await fishingLicenseRepository.DeleteFishingLicenseAsync(fishingLicenseId);
            return result ? Ok(true) : BadRequest("Lizenz konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteFishingLicense)}");
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