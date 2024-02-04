using Application.DataTransferObjects.FishingClub;
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
public class FishingClubsController(IFishingClubRepository fishingClubRepository, ILogger<FishingClubsController> logger) : ControllerBase
{
    // Handle GET request to retrieve all fishing clubs
    [HttpGet]
    public async Task<ActionResult<List<FishingClubDto>>> GetFishingClubs()
    {
        try
        {
            var fishingClubs = await fishingClubRepository.GetFishingClubsAsync();
            return fishingClubs.Count != 0 ? Ok(fishingClubs) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingClubs)}");
        }
    }

    // Handle GET request to retrieve a specific fishing club by ID
    [HttpGet("{fishingClubId:guid}")]
    public async Task<ActionResult<FishingClubDto>> GetFishingClub(Guid fishingClubId)
    {
        try
        {
            var fishingClub = await fishingClubRepository.GetFishingClubByIdAsync(fishingClubId);
            if (fishingClub == null) return NotFound($"No fishingClub found with id: {fishingClubId}");
            return Ok(fishingClub);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingClub)}");
        }
    }

    // Handle POST request to create a new fishing club
    [HttpPost]
    public async Task<ActionResult<FishingClubDto>> CreateFishingClub(CreateFishingClubDto createFishingClubDto)
    {
        try
        {
            var newFishingClub = await fishingClubRepository.CreateFishingClubAsync(createFishingClubDto);
            if (newFishingClub == null) return BadRequest("Could not create new fishing club");
            return Ok(newFishingClub);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateFishingClub)}");
        }
    }

    // Handle PUT request to update an existing fishing club by ID
    [HttpPut("{fishingClubId:guid}")]
    public async Task<ActionResult<FishingClubDto>> UpdateFishingClub(Guid fishingClubId,
        FishingClubDto fishingClubDto)
    {
        try
        {
            if (fishingClubId != fishingClubDto.Id) return BadRequest("Mismatch in Ids");
            var updatedFishingClub =
                await fishingClubRepository.UpdateFishingClubAsync(fishingClubId, fishingClubDto);
            if (updatedFishingClub == null)
                return BadRequest($"Could not update fishing club with id: {fishingClubId}");
            return Ok(updatedFishingClub);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateFishingClub)}");
        }
    }

    // Handle DELETE request to delete a fishing club by ID
    [HttpDelete("{fishingClubId:guid}")]
    public async Task<ActionResult<bool>> DeleteFishingClub(Guid fishingClubId)
    {
        try
        {
            var response = await fishingClubRepository.DeleteFishingClubAsync(fishingClubId);
            return response ? Ok(true) : BadRequest($"Could not delete fishing club with id: {fishingClubId}");
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteFishingClub)}");
        }
    }
}