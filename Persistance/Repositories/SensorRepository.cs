using Application.DataTransferObjects.Sensor;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SensorRepository(DaettwilerPondDbContext context, IMapper mapper) : ISensorRepository
{
    public async Task<List<SensorDto>> GetSensorsAsync()
    {
        var sensors = await context.Sensors
            .ProjectTo<SensorDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return sensors;
    }

    public async Task<SensorDto> GetSensorByIdAsync(Guid sensorId)
    {
        var sensor = await context.Sensors
            .ProjectTo<SensorDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == sensorId);
        return sensor;
    }

    public async Task<SensorDto> GetSensorByDevEuiAsync(string devEui)
    {
        var sensor = await context.Sensors
            .ProjectTo<SensorDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.DevEui == devEui);
        return sensor;
    }


    public async Task<SensorDto> CreatorSensorAsync(CreateSensorDto createSensorDto)
    {
        var sensor = mapper.Map<Sensor>(createSensorDto);
        await context.AddAsync(sensor);
        await context.SaveChangesAsync();
        return mapper.Map<SensorDto>(sensor);
    }

    public async Task<SensorDto> UpdateSensorAsync(UpdateSensorDto updateSensorDto)
    {
        var sensor = await context.Sensors.FirstOrDefaultAsync(s => s.Id == updateSensorDto.Ïd);
        if (sensor == null) return null;
        mapper.Map(updateSensorDto, sensor);
        await context.SaveChangesAsync();
        return mapper.Map<SensorDto>(sensor);
    }

    public async Task<bool> DeleteSensorAsync(Guid sensorId)
    {
        var sensor = await context.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);
        if (sensor == null) return false;
        context.Sensors.Remove(sensor);
        await context.SaveChangesAsync();
        return true;
    }
}