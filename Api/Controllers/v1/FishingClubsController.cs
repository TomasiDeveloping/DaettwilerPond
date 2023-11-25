using Application.DataTransferObjects.FishingClub;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class FishingClubsController : ControllerBase
{
    private readonly IFishingClubRepository _fishingClubRepository;
    private readonly ILogger<FishingClubsController> _logger;

    public FishingClubsController(IFishingClubRepository fishingClubRepository, ILogger<FishingClubsController> logger)
    {
        _fishingClubRepository = fishingClubRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<FishingClubDto>>> GetFishingClubs()
    {
        try
        {
            var fishingClubs = await _fishingClubRepository.GetFishingClubsAsync();
            return fishingClubs.Any() ? Ok(fishingClubs) : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingClubs)}");
        }
    }

    [HttpGet("{fishingClubId:guid}")]
    public async Task<ActionResult<FishingClubDto>> GetFishingClub(Guid fishingClubId)
    {
        try
        {
            var fishingClub = await _fishingClubRepository.GetFishingClubByIdAsync(fishingClubId);
            if (fishingClub == null) return NotFound($"No fishingClub found with id: {fishingClubId}");
            return Ok(fishingClub);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetFishingClub)}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<FishingClubDto>> CreateFishingClub(CreateFishingClubDto createFishingClubDto)
    {
        try
        {
            var newFishingClub = await _fishingClubRepository.CreateFishingClubAsync(createFishingClubDto);
            if (newFishingClub == null) return BadRequest("Could not create new fishing club");
            return Ok(newFishingClub);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateFishingClub)}");
        }
    }

    [HttpPut("{fishingClubId:guid}")]
    public async Task<ActionResult<FishingClubDto>> UpdateFishingClub(Guid fishingClubId,
        FishingClubDto fishingClubDto)
    {
        try
        {
            if (fishingClubId != fishingClubDto.Id) return BadRequest("Mismatch in Ids");
            var updatedFishingClub =
                await _fishingClubRepository.UpdateFishingClubAsync(fishingClubId, fishingClubDto);
            if (updatedFishingClub == null)
                return BadRequest($"Could not update fishing club with id: {fishingClubId}");
            return Ok(updatedFishingClub);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(UpdateFishingClub)}");
        }
    }

    [HttpDelete("{fishingClubId:guid}")]
    public async Task<ActionResult<bool>> DeleteFishingClub(Guid fishingClubId)
    {
        try
        {
            var response = await _fishingClubRepository.DeleteFishingClubAsync(fishingClubId);
            return response ? Ok(true) : BadRequest($"Could not delete fishing club with id: {fishingClubId}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(DeleteFishingClub)}");
        }
    }
}