using Application.DataTransferObjects.FishType;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class FishTypesController : ControllerBase
{
    private readonly IFishTypeRepository _fishTypeRepository;
    private readonly ILogger<FishTypesController> _logger;

    public FishTypesController(IFishTypeRepository fishTypeRepository, ILogger<FishTypesController> logger)
    {
        _fishTypeRepository = fishTypeRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<FishTypeDto>>> GetFishTypes()
    {
        try
        {
            var fishTypes = await _fishTypeRepository.GetFishTypesAsync();
            return fishTypes.Any() ? Ok(fishTypes) : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishTypes)}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<FishTypeDto>> CreateFishType(FishTypeDto fishTypeDto)
    {
        try
        {
            var newFishType = await _fishTypeRepository.CreateFishTypeAsync(fishTypeDto);
            return newFishType == null
                ? BadRequest("Neue Fischart konnte nicht erstellt werden")
                : StatusCode(StatusCodes.Status201Created, newFishType);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateFishType)}");
        }
    }

    [HttpPut("{fishTypeId:guid}")]
    public async Task<ActionResult<FishTypeDto>> UpdateFishType(Guid fishTypeId, FishTypeDto fishTypeDto)
    {
        try
        {
            if (fishTypeId != fishTypeDto.Id) return BadRequest("Fehler in Ids!");
            var updatedFishType = await _fishTypeRepository.UpdateFishTypeAsync(fishTypeId, fishTypeDto);
            if (updatedFishType == null) return BadRequest("Fischart konnte nicht geupdated werden");
            return Ok(updatedFishType);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateFishType)}");
        }
    }

    [HttpDelete("{fishTypeId:guid}")]
    public async Task<ActionResult<bool>> DeleteFishType(Guid fishTypeId)
    {
        try
        {
            var response = await _fishTypeRepository.DeleteFishTypeAsync(fishTypeId);
            return response ? Ok(true) : BadRequest("Fischart konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteFishType)}");
        }
    }
}