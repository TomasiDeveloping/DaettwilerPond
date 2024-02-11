using Application.DataTransferObjects.Catch;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

// Define the route, API version, and authorize the controller
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class OverseersController(
    IFishingLicenseRepository fishingLicenseRepository,
    ILogger<OverseersController> logger) : ControllerBase
{
    [HttpGet("{year:int}")]
    public async Task<ActionResult<DetailYearlyCatch>> GetDetailYearlyCatch(int year)
    {
        try
        {
            var detailYearlyCatch = await fishingLicenseRepository.GetDetailYearlyCatchAsync(year);
            return detailYearlyCatch is null ? NotFound($"No Details for year {year}") : Ok(detailYearlyCatch);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetDetailYearlyCatch)}");
        }
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<DetailYearlyCatch>> GetMemberDetail(Guid userId)
    {
        try
        {
            var detailYearlyCatch = await fishingLicenseRepository.GetOverseerMemberDetailAsync(userId);
            return detailYearlyCatch is null ? NotFound($"No Details for userId {userId}") : Ok(detailYearlyCatch);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetDetailYearlyCatch)}");
        }
    }
}