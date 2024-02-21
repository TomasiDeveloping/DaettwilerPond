using System.Security.Claims;
using Application.DataTransferObjects.FishingLicense;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

// Define the route, API version, and authorize the controller
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class FishingLicensesController(IFishingLicenseRepository fishingLicenseRepository,
    ILogger<FishingLicensesController> logger) : ControllerBase
{
    // Handle GET request to retrieve all fishing licenses
    [HttpGet]
    public async Task<ActionResult<List<FishingLicenseDto>>> GetFishingLicenses()
    {
        try
        {
            var licenses = await fishingLicenseRepository.GetFishingLicensesAsync();
            return licenses.Count != 0 ? Ok(licenses) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingLicenses)}");
        }
    }

    // Handle GET request to retrieve a specific fishing license by ID
    [HttpGet("{fishingLicenseId:guid}")]
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingLicense)}");
        }
    }

    // Handle GET request to retrieve fishing licenses for a specific user
    [HttpGet("Users/{userId:guid}")]
    public async Task<ActionResult<List<FishingLicenseDto>>> GetUserFishingLicenses(Guid userId)
    {
        try
        {
            var userLicenses = await fishingLicenseRepository.GetUserFishingLicenses(userId);
            return userLicenses.Count != 0 ? Ok(userLicenses) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetUserFishingLicenses)}");
        }
    }

    // Handle GET request to retrieve the current user's fishing license for the current year
    [HttpGet("Users/[action]/{userId:guid}")]
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetCurrentUserLicense)}");
        }
    }

    // Handle POST request to create a new fishing license
    [HttpPost]
    public async Task<ActionResult<FishingLicenseDto>> CreateFishingLicense(
        CreateFishingLicenseDto createFishingLicenseDto)
    {
        try
        {
            var newLicense =
                await fishingLicenseRepository.CreateFishingLicenseAsync(createFishingLicenseDto, GetUserFullName());
            if (newLicense == null) return BadRequest("Lizenz konnte nicht erstellt werden");
            return Ok(newLicense);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateFishingLicense)}");
        }
    }

    // Handle PUT request to update an existing fishing license by ID
    [HttpPut("{fishingLicenseId:guid}")]
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateFishingLicense)}");
        }
    }

    // Handle DELETE request to delete a fishing license by ID
    [HttpDelete("{fishingLicenseId:guid}")]
    public async Task<ActionResult<bool>> DeleteFishingLicense(Guid fishingLicenseId)
    {
        try
        {
            var result = await fishingLicenseRepository.DeleteFishingLicenseAsync(fishingLicenseId);
            return result ? Ok(true) : BadRequest("Lizenz konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteFishingLicense)}");
        }
    }

    private string GetUserFullName()
    {
        var email = HttpContext.User.Identity is ClaimsIdentity identity
            ? identity.FindFirst("fullName")?.Value ?? "Unknown"
            : "Unknown";
        return email;
    }
}