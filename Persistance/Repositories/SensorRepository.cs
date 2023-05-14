using Application.DataTransferObjects.Sensor;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;

    public SensorRepository(DaettwilerPondDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<SensorDto>> GetSensorsAsync()
    {
        var sensors = await _context.Sensors
            .ProjectTo<SensorDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return sensors;
    }

    public async Task<SensorDto> GetSensorByIdAsync(Guid sensorId)
    {
        var sensor = await _context.Sensors
            .ProjectTo<SensorDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == sensorId);
        return sensor;
    }

    public async Task<SensorDto> CreatorSensorAsync(CreateSensorDto createSensorDto)
    {
        var sensor = _mapper.Map<Sensor>(createSensorDto);
        await _context.AddAsync(sensor);
        await _context.SaveChangesAsync();
        return _mapper.Map<SensorDto>(sensor);
    }

    public async Task<SensorDto> UpdateSensorAsync(UpdateSensorDto updateSensorDto)
    {
        var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.Id == updateSensorDto.Ïd);
        if (sensor == null) return null;
        _mapper.Map(updateSensorDto, sensor);
        await _context.SaveChangesAsync();
        return _mapper.Map<SensorDto>(sensor);
    }

    public async Task<bool> DeleteSensorAsync(Guid sensorId)
    {
        var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);
        if (sensor == null) return false;
        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
        return true;
    }
}