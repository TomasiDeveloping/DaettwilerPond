using Application.DataTransferObjects.Catch;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class CatchesController(ICatchRepository catchRepository, ILogger<CatchesController> logger) : ControllerBase
{
    [HttpGet("{catchId:guid}")]
    public async Task<ActionResult<CatchDto>> GetCatchById(Guid catchId)
    {
        try
        {
            var fishCatch = await catchRepository.GetCatchAsync(catchId);
            return fishCatch is null
                ? NotFound($"Es wurde kein Fang gefunden mit der Id:{catchId}")
                : Ok(fishCatch);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetCatchById)}");
        }
    }

    [HttpGet("[action]/{licenceId:guid}")]
    public async Task<ActionResult<CatchDto>> GetCatchForCurrentDay(Guid licenceId)
    {
        try
        {
            var currentCatch = await catchRepository.GetCatchForCurrentDayAsync(licenceId);
            return currentCatch is null ? NoContent() : Ok(currentCatch);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetCatchForCurrentDay)}");
        }
    }

    [HttpGet("[action]/{licenceId:guid}")]
    public async Task<ActionResult<CatchDto>> StartFishingDay(Guid licenceId)
    {
        try
        {
            var currentFishDay = await catchRepository.StartCatchDayAsync(licenceId);
            return currentFishDay is null ? BadRequest("Fischertag konnte nicht erstellt werden") : Ok(currentFishDay);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(StartFishingDay)}");
        }
    }

    [HttpGet("[action]/{catchId:guid}")]
    public async Task<ActionResult<CatchDto>> StopFishingDay(Guid catchId)
    {
        try
        {
            var currentFishDay = await catchRepository.StopCatchDayAsync(catchId);
            return currentFishDay is null ? BadRequest("Fischertag konnte nicht gestoppt werden") : Ok(currentFishDay);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(StopFishingDay)}");
        }
    }

    [HttpGet("[action]/{catchId:guid}")]
    public async Task<ActionResult<CatchDto>> ContinueFishingDay(Guid catchId)
    {
        try
        {
            var currentFishDay = await catchRepository.ContinueFishingDayAsync(catchId);
            return currentFishDay is null ? BadRequest("Fischertag konnte nicht neu gestartet werden") : Ok(currentFishDay);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(ContinueFishingDay)}");
        }
    }

    [HttpGet("[action]/{licenceId:guid}")]
    public async Task<ActionResult<YearlyCatch>> GetYearlyCatch(Guid licenceId)
    {
        try
        {
            var yearlyCatch = await catchRepository.GetYearlyCatchAsync(licenceId);
            return yearlyCatch is null ? NoContent() : Ok(yearlyCatch);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetYearlyCatch)}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CatchDto>> CreateCatch(CreateCatchDto createCatchDto)
    {
        try
        {
            var newCatch = await catchRepository.CreateCatchAsync(createCatchDto);
            return newCatch is null
                ? BadRequest("Neuer Fang konnte nicht erstellt werden")
                : StatusCode(StatusCodes.Status201Created, newCatch);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateCatch)}");
        }
    }


    [HttpPut("{catchId:guid}")]
    public async Task<ActionResult<CatchDto>> UpdateCatch(Guid catchId, UpdateCatchDto updateCatchDto)
    {
        try
        {
            if (catchId != updateCatchDto.Id) return BadRequest("Fehler bei Id's");
            var updatedCatch = await catchRepository.UpdateCatchAsync(updateCatchDto);
            return updatedCatch is null ? BadRequest("Fang konnte nicht geupdated werden") : Ok(updatedCatch);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateCatch)}");
        }
    }

    [HttpDelete("{catchId:guid}")]
    public async Task<ActionResult<bool>> DeleteCatch(Guid catchId)
    {
        try
        {
            var deleteResponse = await catchRepository.DeleteCatchAsync(catchId);
            return deleteResponse ? Ok(true) : BadRequest("Fang konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteCatch)}");
        }
    }
}