using Application.DataTransferObjects.SensorType;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

// SensorTypeRepository handles CRUD operations for SensorTypes
public class SensorTypeRepository(DaettwilerPondDbContext context, IMapper mapper) : ISensorTypeRepository
{
    // Get a list of all SensorTypes
    public async Task<List<SensorTypeDto>> GetSensorTypesAsync()
    {
        // Projecting and querying a list of SensorTypeDto
        var sensorTypes = await context.SensorTypes
            .ProjectTo<SensorTypeDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return sensorTypes;
    }

    // Get a SensorType by ID
    public async Task<SensorTypeDto> GetSensorTypeByIdAsync(Guid sensorTypeId)
    {
        // Projecting and querying a SensorTypeDto based on SensorType ID
        var sensorType = await context.SensorTypes
            .ProjectTo<SensorTypeDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.Id == sensorTypeId);
        return sensorType;
    }

    // Create a new SensorType
    public async Task<SensorTypeDto> CreateSensorTypeAsync(CreateSensorTypeDto createSensorTypeDto)
    {
        // Mapping and adding a new SensorType to the database
        var sensorType = mapper.Map<SensorType>(createSensorTypeDto);
        await context.SensorTypes.AddAsync(sensorType);
        await context.SaveChangesAsync();

        return mapper.Map<SensorTypeDto>(sensorType);
    }

    // Update an existing SensorType
    public async Task<SensorTypeDto> UpdateSensorTypeAsync(SensorTypeDto sensorTypeDto)
    {
        // Retrieve and update an existing SensorType
        var sensorType = await context.SensorTypes.FirstOrDefaultAsync(st => st.Id == sensorTypeDto.Id);
        if (sensorType == null) return null;

        // Map values from SensorTypeDto to the existing SensorType entity
        mapper.Map(sensorTypeDto, sensorType);

        // Save changes to the database
        await context.SaveChangesAsync();

        // Return the updated SensorTypeDto
        return mapper.Map<SensorTypeDto>(sensorType);
    }

    // Delete a SensorType by ID
    public async Task<bool> DeleteSensorTypeAsync(Guid sensorTypeId)
    {
        // Retrieve and delete a SensorType by ID
        var sensorType = await context.SensorTypes.FirstOrDefaultAsync(st => st.Id == sensorTypeId);
        if (sensorType == null) return false;

        // Remove the SensorType from the context
        context.SensorTypes.Remove(sensorType);

        // Save changes to the database
        await context.SaveChangesAsync();

        // Return true to indicate successful deletion
        return true;
    }
}