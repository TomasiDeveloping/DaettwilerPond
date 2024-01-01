using Application.DataTransferObjects.Lsn50V2Measurement;

namespace Application.Interfaces;

public interface ILsn50V2MeasurementRepository
{
    Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsAsync();
    Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsBySensorIdAsync(Guid sensorId);
    Task<Lsn50V2MeasurementDto> GetLsn50V2MeasurementByIdAsync(Guid measurementId);

    Task<Lsn50V2TemperatureMeasurementDto> GetLatestMeasurementAsync();
    Task<List<Lsn50V2TemperatureMeasurementDto>> GetTemperatureMeasurementsByDays(int includeDays);

    Task<Lsn50V2MeasurementDto> CreateLsn50V2MeasurementAsync(
        CreateLsn50V2MeasurementDto createLsn50V2MeasurementDto);
    Task<TemperatureHistoryDto> GetTemperatureHistoryAsync();
}