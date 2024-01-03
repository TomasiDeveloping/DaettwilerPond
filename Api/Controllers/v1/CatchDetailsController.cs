using Application.DataTransferObjects.CatchDetail;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class CatchDetailsController(
    ICatchDetailRepository catchDetailRepository,
    ILogger<CatchDetailsController> logger) : ControllerBase
{
    [HttpGet("{catchDetailId:guid}")]
    public async Task<ActionResult<CatchDetailDto>> GetCatchDetail(Guid catchDetailId)
    {
        try
        {
            var catchDetail = await catchDetailRepository.GetCatchDetailAsync(catchDetailId);
            return catchDetail is null
                ? NotFound($"Es wurde kein Fangdetail mit der Id: {catchDetailId} gefunden.")
                : Ok(catchDetail);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetCatchDetail)}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CatchDetailDto>> CreateCatchDetail(CreateCatchDetailDto createCatchDetailDto)
    {
        try
        {
            var newCatchDetail = await catchDetailRepository.CreateCatchDetailAsync(createCatchDetailDto);
            return newCatchDetail is null
                ? BadRequest("Neuer Fangdetail konnte nicht erstellt werden.")
                : StatusCode(StatusCodes.Status201Created, newCatchDetail);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateCatchDetail)}");
        }
    }

    [HttpPut("{catchDetailId:guid}")]
    public async Task<ActionResult<CatchDetailDto>> UpdateCatchDetail(Guid catchDetailId,
        UpdateCatchDetailDto updateCatchDetailDto)
    {
        try
        {
            if (catchDetailId != updateCatchDetailDto.Id) return BadRequest("Fehler in Id's");
            var updatedCatchDetail = await catchDetailRepository.UpdateCatchDetailAsync(updateCatchDetailDto);
            return updatedCatchDetail is null
                ? BadRequest("Fangdetail konnte nicht geupdatet werden.")
                : Ok(updatedCatchDetail);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateCatchDetail)}");
        }
    }

    [HttpDelete("{catchDetailId:guid}")]
    public async Task<ActionResult<bool>> DeleteCatchDetail(Guid catchDetailId)
    {
        try
        {
            var deleteResponse = await catchDetailRepository.DeleteCatchDetailAsync(catchDetailId);
            return deleteResponse ? Ok(true) : BadRequest("Fangdetail konnte nicht gelöscht werden");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteCatchDetail)}");
        }
    }
}