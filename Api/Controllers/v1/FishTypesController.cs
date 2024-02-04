using Application.DataTransferObjects.FishType;
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
public class FishTypesController(IFishTypeRepository fishTypeRepository, ILogger<FishTypesController> logger) : ControllerBase
{

    // Handle GET request to retrieve all fish types
    [HttpGet]
    public async Task<ActionResult<List<FishTypeDto>>> GetFishTypes()
    {
        try
        {
            var fishTypes = await fishTypeRepository.GetFishTypesAsync();
            return fishTypes.Count != 0 ? Ok(fishTypes) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishTypes)}");
        }
    }

    // Handle POST request to create a new fish type
    [HttpPost]
    public async Task<ActionResult<FishTypeDto>> CreateFishType(CreateFishTypeDto createFishTypeDto)
    {
        try
        {
            var newFishType = await fishTypeRepository.CreateFishTypeAsync(createFishTypeDto);
            return newFishType == null
                ? BadRequest("Neue Fischart konnte nicht erstellt werden")
                : StatusCode(StatusCodes.Status201Created, newFishType);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateFishType)}");
        }
    }

    // Handle PUT request to update an existing fish type by ID
    [HttpPut("{fishTypeId:guid}")]
    public async Task<ActionResult<FishTypeDto>> UpdateFishType(Guid fishTypeId, FishTypeDto fishTypeDto)
    {
        try
        {
            if (fishTypeId != fishTypeDto.Id) return BadRequest("Fehler in Ids!");
            var updatedFishType = await fishTypeRepository.UpdateFishTypeAsync(fishTypeId, fishTypeDto);
            if (updatedFishType == null) return BadRequest("Fischart konnte nicht geupdated werden");
            return Ok(updatedFishType);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateFishType)}");
        }
    }

    // Handle DELETE request to delete a fish type by ID
    [HttpDelete("{fishTypeId:guid}")]
    public async Task<ActionResult<bool>> DeleteFishType(Guid fishTypeId)
    {
        try
        {
            var response = await fishTypeRepository.DeleteFishTypeAsync(fishTypeId);
            return response ? Ok(true) : BadRequest("Fischart konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteFishType)}");
        }
    }
}