using Application.DataTransferObjects.Lsn50V2Measurement;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

// Lsn50V2MeasurementRepository handles CRUD operations for LSN50 V2 Measurements
public class Lsn50V2MeasurementRepository(DaettwilerPondDbContext context, IMapper mapper)
    : ILsn50V2MeasurementRepository
{
    // Get a list of all LSN50 V2 measurements
    public async Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsAsync()
    {
        // Projecting and querying a list of Lsn50V2MeasurementDto
        var measurements = await context.Lsn50V2Measurements
            .ProjectTo<Lsn50V2MeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return measurements;
    }

    // Get a list of LSN50 V2 measurements by sensor ID
    public async Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsBySensorIdAsync(Guid sensorId)
    {
        // Projecting and querying a list of Lsn50V2MeasurementDto based on sensor ID
        var measurements = await context.Lsn50V2Measurements
            .ProjectTo<Lsn50V2MeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(l => l.SensorId == sensorId)
            .ToListAsync();
        return measurements;
    }

    // Get an LSN50 V2 measurement by ID
    public async Task<Lsn50V2MeasurementDto> GetLsn50V2MeasurementByIdAsync(Guid measurementId)
    {
        // Projecting and querying an Lsn50V2MeasurementDto based on measurement ID
        var measurement = await context.Lsn50V2Measurements
            .ProjectTo<Lsn50V2MeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == measurementId);
        return measurement;
    }

    // Get the latest temperature measurement
    public async Task<Lsn50V2TemperatureMeasurementDto> GetLatestMeasurementAsync()
    {
        // Querying and projecting the latest temperature measurement
        var temperatureMeasurement = await context.Lsn50V2Measurements
            .OrderByDescending(l => l.ReceivedAt)
            .ProjectTo<Lsn50V2TemperatureMeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return temperatureMeasurement;
    }

    // Get temperature measurements for the last specified number of days
    public async Task<List<Lsn50V2TemperatureMeasurementDto>> GetTemperatureMeasurementsByDays(int includeDays)
    {
        // Filtering temperature measurements based on the specified number of days
        var filterDate = DateTime.Today.AddDays(-includeDays);
        var temperatureMeasurements = await context.Lsn50V2Measurements
            .Where(l => l.ReceivedAt >= filterDate)
            .OrderBy(l => l.ReceivedAt)
            .ProjectTo<Lsn50V2TemperatureMeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return temperatureMeasurements;
    }

    // Create a new LSN50 V2 measurement
    public async Task<Lsn50V2MeasurementDto> CreateLsn50V2MeasurementAsync(
        CreateLsn50V2MeasurementDto createLsn50V2MeasurementDto)
    {
        // Mapping and adding a new Lsn50V2Measurement to the database
        var measurement = mapper.Map<Lsn50V2Measurement>(createLsn50V2MeasurementDto);
        await context.AddAsync(measurement);
        await context.SaveChangesAsync();
        return mapper.Map<Lsn50V2MeasurementDto>(measurement);
    }

    // Get temperature history statistics
    public async Task<TemperatureHistoryDto> GetTemperatureHistoryAsync()
    {
        // Retrieving various temperature statistics for the current day, month, and year
        var date = DateTime.Today;
        var maxRecord = await context.Lsn50V2Measurements.OrderByDescending(x => x.Temperature).FirstOrDefaultAsync();
        var minRecord = await context.Lsn50V2Measurements.OrderBy(x => x.Temperature).FirstOrDefaultAsync();
        var maxMonthTemperature = await context.Lsn50V2Measurements
            .Where(x => x.ReceivedAt.Year == date.Year && x.ReceivedAt.Month == date.Month)
            .MaxAsync(x => x.Temperature);
        var minMonthTemperature = await context.Lsn50V2Measurements
            .Where(x => x.ReceivedAt.Year == date.Year && x.ReceivedAt.Month == date.Month)
            .MinAsync(x => x.Temperature);
        var averageMonthTemperature = await context.Lsn50V2Measurements
            .Where(x => x.ReceivedAt.Year == date.Year && x.ReceivedAt.Month == date.Month)
            .AverageAsync(x => x.Temperature);


        var maxYearTemperature = await context.Lsn50V2Measurements
            .Where(x => x.ReceivedAt.Year == date.Year)
            .MaxAsync(x => x.Temperature);
        var minYearTemperature = await context.Lsn50V2Measurements
            .Where(x => x.ReceivedAt.Year == date.Year)
            .MinAsync(x => x.Temperature);
        var averageYearTemperature = await context.Lsn50V2Measurements
            .Where(x => x.ReceivedAt.Year == date.Year)
            .AverageAsync(x => x.Temperature);

        // Constructing and returning a TemperatureHistoryDto with the obtained statistics
        return new TemperatureHistoryDto
        {
            MaximumTemperatureReceivedTime = maxRecord.ReceivedAt,
            MaximumTemperature = maxRecord.Temperature,
            MinimumTemperatureReceivedTime = minRecord.ReceivedAt,
            MinimumTemperature = minRecord.Temperature,
            MaximumTemperatureYear = maxYearTemperature,
            MaximumTemperatureMonth = maxMonthTemperature,
            MinimumTemperatureYear = minYearTemperature,
            MinimumTemperatureMonth = minMonthTemperature,
            TemperatureAverageYear = averageYearTemperature,
            TemperatureAverageMonth = averageMonthTemperature
        };
    }
}