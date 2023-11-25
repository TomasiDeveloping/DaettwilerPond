using Application.DataTransferObjects.FishType;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class FishTypesController(IFishTypeRepository fishTypeRepository, ILogger<FishTypesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<FishTypeDto>>> GetFishTypes()
    {
        try
        {
            var fishTypes = await fishTypeRepository.GetFishTypesAsync();
            return fishTypes.Any() ? Ok(fishTypes) : NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishTypes)}");
        }
    }

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
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateFishType)}");
        }
    }

    [HttpPut("{fishTypeId}")]
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
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateFishType)}");
        }
    }

    [HttpDelete("{fishTypeId}")]
    public async Task<ActionResult<bool>> DeleteFishType(Guid fishTypeId)
    {
        try
        {
            var response = await fishTypeRepository.DeleteFishTypeAsync(fishTypeId);
            return response ? Ok(true) : BadRequest("Fischart konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteFishType)}");
        }
    }
}