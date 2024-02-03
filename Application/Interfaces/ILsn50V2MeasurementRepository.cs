using Application.DataTransferObjects.Lsn50V2Measurement;

namespace Application.Interfaces;

// Interface for managing LSN50v2D20 sensor measurement-related operations
public interface ILsn50V2MeasurementRepository
{
    // Get a list of all LSN50v2D20 sensor measurements
    Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsAsync();

    // Get a list of LSN50v2D20 sensor measurements associated with a specific sensor ID
    Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsBySensorIdAsync(Guid sensorId);

    // Get a specific LSN50v2D20 sensor measurement by its ID
    Task<Lsn50V2MeasurementDto> GetLsn50V2MeasurementByIdAsync(Guid measurementId);

    // Get the latest temperature measurement
    Task<Lsn50V2TemperatureMeasurementDto> GetLatestMeasurementAsync();

    // Get a list of temperature measurements for the specified number of days
    Task<List<Lsn50V2TemperatureMeasurementDto>> GetTemperatureMeasurementsByDays(int includeDays);

    // Create a new LSN50v2D20 sensor measurement with the specified details
    Task<Lsn50V2MeasurementDto> CreateLsn50V2MeasurementAsync(
        CreateLsn50V2MeasurementDto createLsn50V2MeasurementDto);

    // Get temperature measurement history
    Task<TemperatureHistoryDto> GetTemperatureHistoryAsync();
}