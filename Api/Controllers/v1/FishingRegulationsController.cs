using Application.DataTransferObjects.FishingRegulation;
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
public class FishingRegulationsController(IFishingRegulationRepository fishingRegulationRepository,
    ILogger<FishingRegulationsController> logger) : ControllerBase
{

    // Handle GET request to retrieve all fishing regulations
    [HttpGet]
    public async Task<ActionResult<List<FishingRegulationDto>>> GetFishingRegulations()
    {
        try
        {
            var fishingRegulations = await fishingRegulationRepository.GetFishingRegulationsAsync();
            return fishingRegulations.Count != 0 ? Ok(fishingRegulations) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingRegulations)}");
        }
    }

    // Handle POST request to create a new fishing regulation
    [HttpPost]
    public async Task<ActionResult<FishingRegulationDto>> CreateFishingRegulation(
        CreateFishingRegulationDto createFishingRegulationDto)
    {
        try
        {
            var newFishingRegulation =
                await fishingRegulationRepository.CreateFishingRegulationAsync(createFishingRegulationDto);
            if (newFishingRegulation == null) return BadRequest("Fischereivorschrift konnte nicht erstellt werden");
            return Ok(newFishingRegulation);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateFishingRegulation)}");
        }
    }

    // Handle PUT request to update an existing fishing regulation by ID
    [HttpPut("{fishingRegulationId:guid}")]
    public async Task<ActionResult<FishingRegulationDto>> UpdateFishingRegulation(Guid fishingRegulationId,
        FishingRegulationDto fishingRegulationDto)
    {
        try
        {
            if (fishingRegulationId != fishingRegulationDto.Id) return BadRequest("Fehler mit Id's");
            var updatedFishingRegulation =
                await fishingRegulationRepository.UpdateFishingRegulationAsync(fishingRegulationId,
                    fishingRegulationDto);
            if (updatedFishingRegulation == null)
                return BadRequest("Fischereivorschrift konnte nicht geupdatet werden");
            return Ok(updatedFishingRegulation);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateFishingRegulation)}");
        }
    }

    // Handle DELETE request to delete a fishing regulation by ID
    [HttpDelete("{fishingRegulationId:guid}")]
    public async Task<ActionResult<bool>> DeleteFishingRegulation(Guid fishingRegulationId)
    {
        try
        {
            var response = await fishingRegulationRepository.DeleteFishingRegulationAsync(fishingRegulationId);
            return response ? Ok(true) : BadRequest("Fischereivorschrift konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteFishingRegulation)}");
        }
    }
}