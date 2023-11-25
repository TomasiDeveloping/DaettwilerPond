using Application.DataTransferObjects.FishType;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FishTypeRepository(DaettwilerPondDbContext context, IMapper mapper) : IFishTypeRepository
{
    public async Task<List<FishTypeDto>> GetFishTypesAsync()
    {
        var fishTypes = await context.FishTypes
            .ProjectTo<FishTypeDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return fishTypes;
    }

    public async Task<FishTypeDto> CreateFishTypeAsync(CreateFishTypeDto createFishTypeDto)
    {
        var fishType = mapper.Map<FishType>(createFishTypeDto);
        fishType.Id = Guid.NewGuid();
        await context.FishTypes.AddAsync(fishType);
        await context.SaveChangesAsync();
        return mapper.Map<FishTypeDto>(fishType);
    }

    public async Task<FishTypeDto> UpdateFishTypeAsync(Guid fishTypeId, FishTypeDto fishTypeDto)
    {
        var fishType = await context.FishTypes.FirstOrDefaultAsync(ft => ft.Id == fishTypeId);
        if (fishType == null) return null;
        mapper.Map(fishTypeDto, fishType);
        await context.SaveChangesAsync();
        return mapper.Map<FishTypeDto>(fishType);
    }

    public async Task<bool> DeleteFishTypeAsync(Guid fishTypeId)
    {
        var fishType = await context.FishTypes.FirstOrDefaultAsync(ft => ft.Id == fishTypeId);
        if (fishType == null) return false;
        context.FishTypes.Remove(fishType);
        await context.SaveChangesAsync();
        return true;
    }
}