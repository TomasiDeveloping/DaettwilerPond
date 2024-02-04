using Application.DataTransferObjects.Sensor;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

// SensorRepository handles CRUD operations for Sensors
public class SensorRepository(DaettwilerPondDbContext context, IMapper mapper) : ISensorRepository
{

    // Get a list of all Sensors
    public async Task<List<SensorDto>> GetSensorsAsync()
    {
        // Projecting and querying a list of SensorDto
        var sensors = await context.Sensors
            .ProjectTo<SensorDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return sensors;
    }

    // Get a sensor by ID
    public async Task<SensorDto> GetSensorByIdAsync(Guid sensorId)
    {
        // Projecting and querying a SensorDto based on sensor ID
        var sensor = await context.Sensors
            .ProjectTo<SensorDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == sensorId);
        return sensor;
    }

    // Get a sensor by DevEui
    public async Task<SensorDto> GetSensorByDevEuiAsync(string devEui)
    {
        // Projecting and querying a SensorDto based on DevEui
        var sensor = await context.Sensors
            .ProjectTo<SensorDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.DevEui == devEui);
        return sensor;
    }

    // Create a new sensor
    public async Task<SensorDto> CreatorSensorAsync(CreateSensorDto createSensorDto)
    {
        // Mapping and adding a new Sensor to the database
        var sensor = mapper.Map<Sensor>(createSensorDto);
        await context.AddAsync(sensor);
        await context.SaveChangesAsync();
        return mapper.Map<SensorDto>(sensor);
    }

    // Update an existing sensor
    public async Task<SensorDto> UpdateSensorAsync(UpdateSensorDto updateSensorDto)
    {
        // Retrieve and update an existing sensor
        var sensor = await context.Sensors.FirstOrDefaultAsync(s => s.Id == updateSensorDto.Ïd);
        if (sensor == null) return null;
        mapper.Map(updateSensorDto, sensor);
        await context.SaveChangesAsync();
        return mapper.Map<SensorDto>(sensor);
    }

    // Delete a sensor by ID
    public async Task<bool> DeleteSensorAsync(Guid sensorId)
    {
        // Retrieve and delete a sensor by ID
        var sensor = await context.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);
        if (sensor == null) return false;
        context.Sensors.Remove(sensor);
        await context.SaveChangesAsync();
        return true;
    }
}