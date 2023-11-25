using Application.DataTransferObjects.SensorType;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class SensorTypeRepository(DaettwilerPondDbContext context, IMapper mapper) : ISensorTypeRepository
{
    public async Task<List<SensorTypeDto>> GetSensorTypesAsync()
    {
        var sensorTypes = await context.SensorTypes
            .ProjectTo<SensorTypeDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return sensorTypes;
    }

    public async Task<SensorTypeDto> GetSensorTypeByIdAsync(Guid sensorTypeId)
    {
        var sensorType = await context.SensorTypes
            .ProjectTo<SensorTypeDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.Id == sensorTypeId);
        return sensorType;
    }

    public async Task<SensorTypeDto> CreateSensorTypeAsync(CreateSensorTypeDto createSensorTypeDto)
    {
        var sensorType = mapper.Map<SensorType>(createSensorTypeDto);
        await context.SensorTypes.AddAsync(sensorType);
        await context.SaveChangesAsync();
        return mapper.Map<SensorTypeDto>(sensorType);
    }

    public async Task<SensorTypeDto> UpdateSensorTypeAsync(SensorTypeDto sensorTypeDto)
    {
        var sensorType = await context.SensorTypes.FirstOrDefaultAsync(st => st.Id == sensorTypeDto.Id);
        if (sensorType == null) return null;
        mapper.Map(sensorTypeDto, sensorType);
        await context.SaveChangesAsync();
        return mapper.Map<SensorTypeDto>(sensorType);
    }

    public async Task<bool> DeleteSensorTypeAsync(Guid sensorTypeId)
    {
        var sensorType = await context.SensorTypes.FirstOrDefaultAsync(st => st.Id == sensorTypeId);
        if (sensorType == null) return false;
        context.SensorTypes.Remove(sensorType);
        await context.SaveChangesAsync();
        return true;
    }
}