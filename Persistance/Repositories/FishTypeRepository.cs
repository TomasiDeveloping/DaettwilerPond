using Application.DataTransferObjects.FishType;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

// FishTypeRepository handles CRUD operations for FishTypes
public class FishTypeRepository(DaettwilerPondDbContext context, IMapper mapper) : IFishTypeRepository
{
    // Get a list of all fish types
    public async Task<List<FishTypeDto>> GetFishTypesAsync()
    {
        // Projecting and querying a list of FishTypeDto
        var fishTypes = await context.FishTypes
            .ProjectTo<FishTypeDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return fishTypes;
    }

    // Create a new fish type
    public async Task<FishTypeDto> CreateFishTypeAsync(CreateFishTypeDto createFishTypeDto)
    {
        // Mapping and adding a new FishType to the database
        var fishType = mapper.Map<FishType>(createFishTypeDto);
        fishType.Id = Guid.NewGuid();
        await context.FishTypes.AddAsync(fishType);
        await context.SaveChangesAsync();
        return mapper.Map<FishTypeDto>(fishType);
    }

    // Update an existing fish type
    public async Task<FishTypeDto> UpdateFishTypeAsync(Guid fishTypeId, FishTypeDto fishTypeDto)
    {
        // Finding the fish type to update
        var fishType = await context.FishTypes.FirstOrDefaultAsync(ft => ft.Id == fishTypeId);
        if (fishType == null) return null;

        // Mapping and updating the fish type
        mapper.Map(fishTypeDto, fishType);
        await context.SaveChangesAsync();
        return mapper.Map<FishTypeDto>(fishType);
    }

    // Delete a fish type by ID
    public async Task<bool> DeleteFishTypeAsync(Guid fishTypeId)
    {
        // Finding the fish type to delete
        var fishType = await context.FishTypes.FirstOrDefaultAsync(ft => ft.Id == fishTypeId);
        if (fishType == null) return false;

        // Removing the fish type and saving changes
        context.FishTypes.Remove(fishType);
        await context.SaveChangesAsync();

        // Returning true to indicate successful deletion
        return true;
    }
}