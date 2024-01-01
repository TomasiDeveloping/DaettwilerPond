using Application.DataTransferObjects.Lsn50V2Measurement;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class Lsn50V2MeasurementsController(ILsn50V2MeasurementRepository lsn50V2MeasurementRepository,
    ILogger<Lsn50V2MeasurementsController> logger) : ControllerBase
{
    [HttpGet("[action]")]
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
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetLatestTemperatureMeasurement)}");
        }
    }

    [HttpGet("[action]/{includeDays}")]
    public async Task<ActionResult<List<Lsn50V2TemperatureMeasurementDto>>> GetTemperatureMeasurementByDay(
        int includeDays)
    {
        try
        {
            var temperatureMeasurements =
                await lsn50V2MeasurementRepository.GetTemperatureMeasurementsByDays(includeDays);
            return temperatureMeasurements.Any() ? Ok(temperatureMeasurements) : NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetTemperatureMeasurementByDay)}");
        }
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<TemperatureHistoryDto>> GetHistoryData()
    {
        try
        {
            var historyData = await lsn50V2MeasurementRepository.GetTemperatureHistoryAsync();
            return historyData is not null ? Ok(historyData) : NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetHistoryData)}");
        }
    }
}