using Application.DataTransferObjects.Lsn50V2Measurement;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

// Define the route, API version, and use ApiController attribute
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
[ApiVersion("1.0")]
public class Lsn50V2MeasurementsController(ILsn50V2MeasurementRepository lsn50V2MeasurementRepository,
    ILogger<Lsn50V2MeasurementsController> logger) : ControllerBase
{

    // Handle GET request to retrieve the latest temperature measurement
    [HttpGet]
    public async Task<ActionResult<Lsn50V2TemperatureMeasurementDto>> GetLatestTemperatureMeasurement()
    {
        try
        {
            var temperatureMeasurement = await lsn50V2MeasurementRepository.GetLatestMeasurementAsync();
            if (temperatureMeasurement == null) return NoContent();
            return Ok(temperatureMeasurement);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetLatestTemperatureMeasurement)}");
        }
    }

    // Handle GET request to retrieve temperature measurements for a specified number of days
    [HttpGet("{includeDays:int}")]
    public async Task<ActionResult<List<Lsn50V2TemperatureMeasurementDto>>> GetTemperatureMeasurementByDay(
        int includeDays)
    {
        try
        {
            var temperatureMeasurements =
                await lsn50V2MeasurementRepository.GetTemperatureMeasurementsByDays(includeDays);
            return temperatureMeasurements.Count != 0 ? Ok(temperatureMeasurements) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetTemperatureMeasurementByDay)}");
        }
    }

    // Handle GET request to retrieve temperature history data
    [HttpGet]
    public async Task<ActionResult<TemperatureHistoryDto>> GetHistoryData()
    {
        try
        {
            var historyData = await lsn50V2MeasurementRepository.GetTemperatureHistoryAsync();
            return historyData is not null ? Ok(historyData) : NoContent();
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetHistoryData)}");
        }
    }
}