using Application.DataTransferObjects.Sensor;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

// Define the route, API version, and use ApiController attribute
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class SensorsController(ISensorRepository sensorRepository, ILogger<SensorsController> logger) : ControllerBase
{
    // Handle GET request to retrieve all sensors
    [HttpGet]
    public async Task<ActionResult<List<SensorDto>>> GetSensors()
    {
        try
        {
            var sensors = await sensorRepository.GetSensorsAsync();
            return sensors.Count != 0 ? Ok(sensors) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetSensors)}");
        }
    }

    // Handle GET request to retrieve a specific sensor by ID
    [HttpGet("{sensorId:guid}")]
    public async Task<ActionResult<SensorDto>> GetSensorById(Guid sensorId)
    {
        try
        {
            var sensor = await sensorRepository.GetSensorByIdAsync(sensorId);
            if (sensor == null) return NotFound($"No sensor found with id: {sensorId}");
            return Ok(sensor);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetSensorById)}");
        }
    }

    // Handle POST request to create a new sensor
    [HttpPost]
    public async Task<ActionResult<SensorDto>> CreateSensor(CreateSensorDto createSensorDto)
    {
        try
        {
            var newSensor = await sensorRepository.CreatorSensorAsync(createSensorDto);
            if (newSensor == null) return BadRequest("Could not create new sensor");
            return Ok(newSensor);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateSensor)}");
        }
    }
}