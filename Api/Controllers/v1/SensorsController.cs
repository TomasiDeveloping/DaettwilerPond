using Application.DataTransferObjects.Sensor;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class SensorsController : ControllerBase
{
    private readonly ILogger<SensorsController> _logger;
    private readonly ISensorRepository _sensorRepository;

    public SensorsController(ISensorRepository sensorRepository, ILogger<SensorsController> logger)
    {
        _sensorRepository = sensorRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<SensorDto>>> GetSensors()
    {
        try
        {
            var sensors = await _sensorRepository.GetSensorsAsync();
            return sensors.Any() ? Ok(sensors) : NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetSensors)}");
        }
    }

    [HttpGet("{sensorId:guid}")]
    public async Task<ActionResult<SensorDto>> GetSensorById(Guid sensorId)
    {
        try
        {
            var sensor = await _sensorRepository.GetSensorByIdAsync(sensorId);
            if (sensor == null) return NotFound($"No sensor found with id: {sensorId}");
            return Ok(sensor);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetSensorById)}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<SensorDto>> CreateSensor(CreateSensorDto createSensorDto)
    {
        try
        {
            var newSensor = await _sensorRepository.CreatorSensorAsync(createSensorDto);
            if (newSensor == null) return BadRequest("Could not create new sensor");
            return Ok(newSensor);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(CreateSensor)}");
        }
    }
}