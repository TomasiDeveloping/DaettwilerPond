using Application.DataTransferObjects.Lsn50V2Measurement;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class Lsn50V2MeasurementsController : ControllerBase
    {
        private readonly ILsn50V2MeasurementRepository _lsn50V2MeasurementRepository;
        private readonly ILogger<Lsn50V2MeasurementsController> _logger;

        public Lsn50V2MeasurementsController(ILsn50V2MeasurementRepository lsn50V2MeasurementRepository, ILogger<Lsn50V2MeasurementsController> logger)
        {
            _lsn50V2MeasurementRepository = lsn50V2MeasurementRepository;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<Lsn50V2TemperatureMeasurementDto>> GetLatestTemperatureMeasurement()
        {
            try
            {
                var temperatureMeasurement = await _lsn50V2MeasurementRepository.GetLatestMeasurementAsync();
                if (temperatureMeasurement == null) return NoContent();
                return Ok(temperatureMeasurement);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Something went wrong in {nameof(GetLatestTemperatureMeasurement)}");
            }
        }

        [HttpGet("[action]/{includeDays:int}")]
        public async Task<ActionResult<List<Lsn50V2TemperatureMeasurementDto>>> GetTemperatureMeasurementByDay(
            int includeDays)
        {
            try
            {
                var temperatureMeasurements =
                    await _lsn50V2MeasurementRepository.GetTemperatureMeasurementsByDays(includeDays);
                return temperatureMeasurements.Any() ? Ok(temperatureMeasurements) : NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Something went wrong in {nameof(GetTemperatureMeasurementByDay)}");
            }
        }
    }
}
