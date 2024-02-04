using Application.DataTransferObjects.SensorType;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

// Define the route, API version, and use ApiController attribute
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class SensorTypesController(ISensorTypeRepository sensorTypeRepository, ILogger<SensorTypesController> logger) : ControllerBase
{

    // Handle GET request to retrieve all sensor types
    [HttpGet]
    public async Task<ActionResult<List<SensorTypeDto>>> GetSensorTypes()
    {
        try
        {
            var sensorTypes = await sensorTypeRepository.GetSensorTypesAsync();
            return sensorTypes.Count != 0 ? Ok(sensorTypes) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetSensorTypes)}");
        }
    }

    // Handle GET request to retrieve a specific sensor type by ID
    [HttpGet("{sensorTypeId:guid}")]
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetSensorType)}");
        }
    }

    // Handle POST request to create a new sensor type
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
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateSensorType)}");
        }
    }
}