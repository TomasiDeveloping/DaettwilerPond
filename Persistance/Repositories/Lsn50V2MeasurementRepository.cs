using Application.DataTransferObjects.Lsn50V2Measurement;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class Lsn50V2MeasurementRepository : ILsn50V2MeasurementRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;

    public Lsn50V2MeasurementRepository(DaettwilerPondDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsAsync()
    {
        var measurements = await _context.Lsn50V2Measurements
            .ProjectTo<Lsn50V2MeasurementDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return measurements;
    }

    public async Task<List<Lsn50V2MeasurementDto>> GetLsn50V2MeasurementsBySensorIdAsync(Guid sensorId)
    {
        var measurements = await _context.Lsn50V2Measurements
            .ProjectTo<Lsn50V2MeasurementDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(l => l.SensorId == sensorId)
            .ToListAsync();
        return measurements;
    }

    public async Task<Lsn50V2MeasurementDto> GetLsn50V2MeasurementByIdAsync(Guid measurementId)
    {
        var measurement = await _context.Lsn50V2Measurements
            .ProjectTo<Lsn50V2MeasurementDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == measurementId);
        return measurement;
    }

    public async Task<Lsn50V2TemperatureMeasurementDto> GetLatestMeasurementAsync()
    {
        var temperatureMeasurement = await _context.Lsn50V2Measurements
            .OrderByDescending(l => l.ReceivedAt)
            .ProjectTo<Lsn50V2TemperatureMeasurementDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return temperatureMeasurement;
    }

    public async Task<List<Lsn50V2TemperatureMeasurementDto>> GetTemperatureMeasurementsByDays(int includeDays)
    {
        var filterDate = DateTime.Today.AddDays(-includeDays);
        var temperatureMeasurements = await _context.Lsn50V2Measurements
            .Where(l => l.ReceivedAt >= filterDate)
            .OrderBy(l => l.ReceivedAt)
            .ProjectTo<Lsn50V2TemperatureMeasurementDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return temperatureMeasurements;
    }

    public async Task<Lsn50V2MeasurementDto> CreateLsn50V2MeasurementAsync(
        CreateLsn50V2MeasurementDto createLsn50V2MeasurementDto)
    {
        var measurement = _mapper.Map<Lsn50V2Measurement>(createLsn50V2MeasurementDto);
        await _context.AddAsync(measurement);
        await _context.SaveChangesAsync();
        return _mapper.Map<Lsn50V2MeasurementDto>(measurement);
    }
}