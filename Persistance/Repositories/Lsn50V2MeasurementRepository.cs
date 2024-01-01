using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Application.DataTransferObjects.Lsn50V2Measurement;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class Lsn50V2MeasurementRepository(DaettwilerPondDbContext context, IMapper mapper) : ILsn50V2MeasurementRepository
{
    public async Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsAsync()
    {
        var measurements = await context.Lsn50V2Measurements
            .ProjectTo<Lsn50V2MeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return measurements;
    }

    public async Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsBySensorIdAsync(Guid sensorId)
    {
        var measurements = await context.Lsn50V2Measurements
            .ProjectTo<Lsn50V2MeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(l => l.SensorId == sensorId)
            .ToListAsync();
        return measurements;
    }

    public async Task<Lsn50V2MeasurementDto> GetLsn50V2MeasurementByIdAsync(Guid measurementId)
    {
        var measurement = await context.Lsn50V2Measurements
            .ProjectTo<Lsn50V2MeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == measurementId);
        return measurement;
    }

    public async Task<Lsn50V2TemperatureMeasurementDto> GetLatestMeasurementAsync()
    {
        var temperatureMeasurement = await context.Lsn50V2Measurements
            .OrderByDescending(l => l.ReceivedAt)
            .ProjectTo<Lsn50V2TemperatureMeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return temperatureMeasurement;
    }

    public async Task<List<Lsn50V2TemperatureMeasurementDto>> GetTemperatureMeasurementsByDays(int includeDays)
    {
        var filterDate = DateTime.Today.AddDays(-includeDays);
        var temperatureMeasurements = await context.Lsn50V2Measurements
            .Where(l => l.ReceivedAt >= filterDate)
            .OrderBy(l => l.ReceivedAt)
            .ProjectTo<Lsn50V2TemperatureMeasurementDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return temperatureMeasurements;
    }

    public async Task<Lsn50V2MeasurementDto> CreateLsn50V2MeasurementAsync(
        CreateLsn50V2MeasurementDto createLsn50V2MeasurementDto)
    {
        var measurement = mapper.Map<Lsn50V2Measurement>(createLsn50V2MeasurementDto);
        await context.AddAsync(measurement);
        await context.SaveChangesAsync();
        return mapper.Map<Lsn50V2MeasurementDto>(measurement);
    }

    public async Task<TemperatureHistoryDto> GetTemperatureHistoryAsync()
    {
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

        return new TemperatureHistoryDto()
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
            TemperatureAverageMonth = averageMonthTemperature,
        };
    }
}