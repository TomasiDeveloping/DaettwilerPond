using Application.DataTransferObjects.FishingClub;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FishingClubRepository(DaettwilerPondDbContext context, IMapper mapper) : IFishingClubRepository
{
    public async Task<List<FishingClubDto>> GetFishingClubsAsync()
    {
        var fishingClubs = await context.FishingClubs
            .ProjectTo<FishingClubDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return fishingClubs;
    }

    public async Task<FishingClubDto> GetFishingClubByIdAsync(Guid fishingClubId)
    {
        var fishingClub = await context.FishingClubs
            .ProjectTo<FishingClubDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(fc => fc.Id == fishingClubId);
        return fishingClub;
    }

    public async Task<FishingClubDto> CreateFishingClubAsync(CreateFishingClubDto createFishingClubDto)
    {
        var fishingClub = mapper.Map<FishingClub>(createFishingClubDto);
        await context.FishingClubs.AddAsync(fishingClub);
        await context.SaveChangesAsync();
        return mapper.Map<FishingClubDto>(fishingClub);
    }

    public async Task<FishingClubDto> UpdateFishingClubAsync(Guid fishingClubId, FishingClubDto fishingClubDto)
    {
        var fishingClub = await context.FishingClubs.FirstOrDefaultAsync(fc => fc.Id == fishingClubId);
        if (fishingClub == null) return null;
        mapper.Map(fishingClubDto, fishingClub);
        await context.SaveChangesAsync();
        return mapper.Map<FishingClubDto>(fishingClub);
    }

    public async Task<bool> DeleteFishingClubAsync(Guid fishingClubId)
    {
        var fishingClub = await context.FishingClubs.FirstOrDefaultAsync(fc => fc.Id == fishingClubId);
        if (fishingClub == null) return false;
        context.FishingClubs.Remove(fishingClub);
        await context.SaveChangesAsync();
        return true;
    }
}