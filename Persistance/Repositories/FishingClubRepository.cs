using Application.DataTransferObjects.FishingClub;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

// FishingClubRepository handles CRUD operations for FishingClubs
public class FishingClubRepository(DaettwilerPondDbContext context, IMapper mapper) : IFishingClubRepository
{
    // Get a list of all fishing clubs
    public async Task<List<FishingClubDto>> GetFishingClubsAsync()
    {
        // Projecting and querying a list of FishingClubDto
        var fishingClubs = await context.FishingClubs
            .ProjectTo<FishingClubDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return fishingClubs;
    }

    // Get a specific fishing club by ID
    public async Task<FishingClubDto> GetFishingClubByIdAsync(Guid fishingClubId)
    {
        // Projecting and querying a specific FishingClubDto by ID
        var fishingClub = await context.FishingClubs
            .ProjectTo<FishingClubDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(fc => fc.Id == fishingClubId);
        return fishingClub;
    }

    // Create a new fishing club
    public async Task<FishingClubDto> CreateFishingClubAsync(CreateFishingClubDto createFishingClubDto)
    {
        // Mapping and adding a new FishingClub to the database
        var fishingClub = mapper.Map<FishingClub>(createFishingClubDto);
        await context.FishingClubs.AddAsync(fishingClub);
        await context.SaveChangesAsync();
        return mapper.Map<FishingClubDto>(fishingClub);
    }

    // Update an existing fishing club
    public async Task<FishingClubDto> UpdateFishingClubAsync(Guid fishingClubId, FishingClubDto fishingClubDto)
    {
        // Finding the fishing club to update
        var fishingClub = await context.FishingClubs.FirstOrDefaultAsync(fc => fc.Id == fishingClubId);
        if (fishingClub == null) return null;

        // Mapping and updating the fishing club
        mapper.Map(fishingClubDto, fishingClub);
        await context.SaveChangesAsync();
        return mapper.Map<FishingClubDto>(fishingClub);
    }

    // Delete a fishing club by ID
    public async Task<bool> DeleteFishingClubAsync(Guid fishingClubId)
    {
        // Finding the fishing club to delete
        var fishingClub = await context.FishingClubs.FirstOrDefaultAsync(fc => fc.Id == fishingClubId);
        if (fishingClub == null) return false;

        // Removing the fishing club and saving changes
        context.FishingClubs.Remove(fishingClub);
        await context.SaveChangesAsync();
        return true;
    }
}