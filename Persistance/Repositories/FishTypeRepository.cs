using Application.DataTransferObjects.FishType;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FishTypeRepository : IFishTypeRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;

    public FishTypeRepository(DaettwilerPondDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FishTypeDto>> GetFishTypesAsync()
    {
        var fishTypes = await _context.FishTypes
            .ProjectTo<FishTypeDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return fishTypes;
    }

    public async Task<FishTypeDto> CreateFishTypeAsync(CreateFishTypeDto createFishTypeDto)
    {
        var fishType = _mapper.Map<FishType>(createFishTypeDto);
        fishType.Id = Guid.NewGuid();
        await _context.FishTypes.AddAsync(fishType);
        await _context.SaveChangesAsync();
        return _mapper.Map<FishTypeDto>(fishType);
    }

    public async Task<FishTypeDto> UpdateFishTypeAsync(Guid fishTypeId, FishTypeDto fishTypeDto)
    {
        var fishType = await _context.FishTypes.FirstOrDefaultAsync(ft => ft.Id == fishTypeId);
        if (fishType == null) return null;
        _mapper.Map(fishTypeDto, fishType);
        await _context.SaveChangesAsync();
        return _mapper.Map<FishTypeDto>(fishType);
    }

    public async Task<bool> DeleteFishTypeAsync(Guid fishTypeId)
    {
        var fishType = await _context.FishTypes.FirstOrDefaultAsync(ft => ft.Id == fishTypeId);
        if (fishType == null) return false;
        _context.FishTypes.Remove(fishType);
        await _context.SaveChangesAsync();
        return true;
    }
}