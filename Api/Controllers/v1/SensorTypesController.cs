using Application.DataTransferObjects.SensorType;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class SensorTypesController(ISensorTypeRepository sensorTypeRepository, ILogger<SensorTypesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SensorTypeDto>>> GetSensorTypes()
    {
        try
        {
            var sensorTypes = await sensorTypeRepository.GetSensorTypesAsync();
            return sensorTypes.Any() ? Ok(sensorTypes) : NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetSensorTypes)}");
        }
    }

    [HttpGet("{sensorTypeId}")]
    public async Task<ActionResult<SensorTypeDto>> GetSensorType(Guid sensorTypeId)
    {
        try
        {
            var sensorType = await sensorTypeRepository.GetSensorTypeByIdAsync(sensorTypeId);
            if (sensorType == null) return NotFound($"No sensorType found with id: {sensorTypeId}");
            return Ok(sensorType);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetSensorType)}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<SensorTypeDto>> CreateSensorType(CreateSensorTypeDto createSensorTypeDto)
    {
        try
        {
            var newSensorType = await sensorTypeRepository.CreateSensorTypeAsync(createSensorTypeDto);
            if (newSensorType == null) return BadRequest("Could not create sensorType");
            return Ok(newSensorType);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateSensorType)}");
        }
    }
}