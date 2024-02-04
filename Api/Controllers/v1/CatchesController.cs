using System.Globalization;
using Application.DataTransferObjects.Catch;
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
public class CatchesController(ICatchRepository catchRepository, ILogger<CatchesController> logger) : ControllerBase
{
    // Handle GET request to retrieve a specific catch by ID
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetCatchById)}");
        }
    }

    // Handle GET request to retrieve the catch for the current day
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetCatchForCurrentDay)}");
        }
    }

    // Handle GET request to start a fishing day
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(StartFishingDay)}");
        }
    }

    // Handle GET request to stop a fishing day
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(StopFishingDay)}");
        }
    }

    // Handle GET request to continue a fishing day
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(ContinueFishingDay)}");
        }
    }

    // Handle GET request to get yearly catch statistics
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetYearlyCatch)}");
        }
    }

    // Handle GET request to check if a catch date exists
    [HttpGet("[action]/{licenceId:guid}/{catchDate}")]
    public async Task<ActionResult<bool>> CheckCatchDateExists(Guid licenceId, string catchDate)
    {
        try
        {
            var date = DateTime.ParseExact(catchDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return await catchRepository.CheckCatchDateExistsAsync(licenceId, date);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CheckCatchDateExists)}");
        }
    }

    // Handle GET request to get detailed yearly catch statistics
    [HttpGet("[action]/{licenceId:guid}")]
    public async Task<ActionResult<List<DetailYearlyCatch>>> GetDetailYearlyCatches(Guid licenceId)
    {
        try
        {
            var detailCatches = await catchRepository.GetDetailYearlyCatchAsync(licenceId);
            return detailCatches.Count != 0 ? Ok(detailCatches) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetDetailYearlyCatches)}");
        }
    }

    // Handle GET request to get catches for a specific month
    [HttpGet("[action]/{licenceId:guid}/{month:int}")]
    public async Task<ActionResult<List<CatchDto>>> GetCatchesForMonth(Guid licenceId, int month)
    {
        try
        {
            var monthCatches = await catchRepository.GetCatchesForMonthAsync(licenceId, month);
            return monthCatches.Count != 0 ? Ok(monthCatches) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetCatchesForMonth)}");
        }
    }

    // Handle POST request to create a new catch
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateCatch)}");
        }
    }

    // Handle PUT request to update an existing catch
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateCatch)}");
        }
    }

    // Handle DELETE request to delete a catch by ID
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
            // Log and return a generic error 
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteCatch)}");
        }
    }
}