using Application.DataTransferObjects.Lsn50V2Lifecycle;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class Lsn50V2LifecycleRepository : ILsn50V2LifecycleRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;

    public Lsn50V2LifecycleRepository(DaettwilerPondDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Lsn50V2LifecycleDto>> GetLsn50V2LifecyclesAsync()
    {
        var lifecycles = await _context.Lsn50V2Lifecycles
            .ProjectTo<Lsn50V2LifecycleDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return lifecycles;
    }

    public async Task<List<Lsn50V2LifecycleDto>> GetLsn50V2LifecyclesBySensorIdAsync(Guid sensorId)
    {
        var lifecycles = await _context.Lsn50V2Lifecycles
            .ProjectTo<Lsn50V2LifecycleDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(l => l.SensorId == sensorId)
            .ToListAsync();
        return lifecycles;
    }

    public async Task<Lsn50V2LifecycleDto> GetLsn50V2LifecycleByIdAsync(Guid id)
    {
        var lifecycle = await _context.Lsn50V2Lifecycles
            .ProjectTo<Lsn50V2LifecycleDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);
        return lifecycle;
    }

    public async Task<Lsn50V2LifecycleDto> CreateLsn50V2LifecycleAsync(
        CreateLsn50V2LifecycleDto createLsn50V2LifecycleDto)
    {
        var lifecycle = _mapper.Map<Lsn50V2Lifecycle>(createLsn50V2LifecycleDto);
        await _context.Lsn50V2Lifecycles.AddAsync(lifecycle);
        await _context.SaveChangesAsync();
        return _mapper.Map<Lsn50V2LifecycleDto>(lifecycle);
    }
}