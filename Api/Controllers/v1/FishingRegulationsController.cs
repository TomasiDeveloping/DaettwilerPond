using Application.DataTransferObjects.FishingRegulation;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class FishingRegulationsController : ControllerBase
{
    private readonly IFishingRegulationRepository _fishingRegulationRepository;
    private readonly ILogger<FishingRegulationsController> _logger;

    public FishingRegulationsController(IFishingRegulationRepository fishingRegulationRepository,
        ILogger<FishingRegulationsController> logger)
    {
        _fishingRegulationRepository = fishingRegulationRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<FishingRegulationDto>>> GetFishingRegulations()
    {
        try
        {
            var fishingRegulations = await _fishingRegulationRepository.GetFishingRegulationsAsync();
            return fishingRegulations.Any() ? Ok(fishingRegulations) : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingRegulations)}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<FishingRegulationDto>> CreateFishingRegulation(
        FishingRegulationDto fishingRegulationDto)
    {
        try
        {
            var newFishingRegulation =
                await _fishingRegulationRepository.CreateFishingRegulationAsync(fishingRegulationDto);
            if (newFishingRegulation == null) return BadRequest("Fischereivorschrift konnte nicht erstellt werden");
            return Ok(newFishingRegulation);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateFishingRegulation)}");
        }
    }

    [HttpPut("{fishingRegulationId:guid}")]
    public async Task<ActionResult<FishingRegulationDto>> UpdateFishingRegulation(Guid fishingRegulationId,
        FishingRegulationDto fishingRegulationDto)
    {
        try
        {
            if (fishingRegulationId != fishingRegulationDto.Id) return BadRequest("Fehler mit Id's");
            var updatedFishingRegulation =
                await _fishingRegulationRepository.UpdateFishingRegulationAsync(fishingRegulationId,
                    fishingRegulationDto);
            if (updatedFishingRegulation == null)
                return BadRequest("Fischereivorschrift konnte nicht geupdatet werden");
            return Ok(updatedFishingRegulation);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateFishingRegulation)}");
        }
    }

    [HttpDelete("{fishingRegulationId:guid}")]
    public async Task<ActionResult<bool>> DeleteFishingRegulation(Guid fishingRegulationId)
    {
        try
        {
            var response = await _fishingRegulationRepository.DeleteFishingRegulationAsync(fishingRegulationId);
            return response ? Ok(true) : BadRequest("Fischereivorschrift konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteFishingRegulation)}");
        }
    }
}