using Application.DataTransferObjects.Lsn50V2Lifecycle;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

// Lsn50V2LifecycleRepository handles CRUD operations for LSN50 V2 Lifecycles
public class Lsn50V2LifecycleRepository(DaettwilerPondDbContext context, IMapper mapper) : ILsn50V2LifecycleRepository
{

    // Get a list of all LSN50 V2 lifecycles
    public async Task<List<Lsn50V2LifecycleDto>> GetLsn50V2LifecyclesAsync()
    {
        // Projecting and querying a list of Lsn50V2LifecycleDto
        var lifecycles = await context.Lsn50V2Lifecycles
            .ProjectTo<Lsn50V2LifecycleDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return lifecycles;
    }

    // Get a list of LSN50 V2 lifecycles by sensor ID
    public async Task<List<Lsn50V2LifecycleDto>> GetLsn50V2LifecyclesBySensorIdAsync(Guid sensorId)
    {
        // Projecting and querying a list of Lsn50V2LifecycleDto based on sensor ID
        var lifecycles = await context.Lsn50V2Lifecycles
            .ProjectTo<Lsn50V2LifecycleDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(l => l.SensorId == sensorId)
            .ToListAsync();
        return lifecycles;
    }

    // Get an LSN50 V2 lifecycle by ID
    public async Task<Lsn50V2LifecycleDto> GetLsn50V2LifecycleByIdAsync(Guid id)
    {
        // Projecting and querying an Lsn50V2LifecycleDto based on lifecycle ID
        var lifecycle = await context.Lsn50V2Lifecycles
            .ProjectTo<Lsn50V2LifecycleDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);
        return lifecycle;
    }

    // Create a new LSN50 V2 lifecycle
    public async Task<Lsn50V2LifecycleDto> CreateLsn50V2LifecycleAsync(
        CreateLsn50V2LifecycleDto createLsn50V2LifecycleDto)
    {
        // Mapping and adding a new Lsn50V2Lifecycle to the database
        var lifecycle = mapper.Map<Lsn50V2Lifecycle>(createLsn50V2LifecycleDto);
        await context.Lsn50V2Lifecycles.AddAsync(lifecycle);
        await context.SaveChangesAsync();
        return mapper.Map<Lsn50V2LifecycleDto>(lifecycle);
    }
}