using Application.DataTransferObjects.FishingClub;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FishingClubRepository : IFishingClubRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;

    public FishingClubRepository(DaettwilerPondDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FishingClubDto>> GetFishingClubsAsync()
    {
        var fishingClubs = await _context.FishingClubs
            .ProjectTo<FishingClubDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return fishingClubs;
    }

    public async Task<FishingClubDto> GetFishingClubByIdAsync(Guid fishingClubId)
    {
        var fishingClub = await _context.FishingClubs
            .ProjectTo<FishingClubDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(fc => fc.Id == fishingClubId);
        return fishingClub;
    }

    public async Task<FishingClubDto> CreateFishingClubAsync(CreateFishingClubDto createFishingClubDto)
    {
        var fishingClub = _mapper.Map<FishingClub>(createFishingClubDto);
        await _context.FishingClubs.AddAsync(fishingClub);
        await _context.SaveChangesAsync();
        return _mapper.Map<FishingClubDto>(fishingClub);
    }

    public async Task<FishingClubDto> UpdateFishingClubAsync(Guid fishingClubId, FishingClubDto fishingClubDto)
    {
        var fishingClub = await _context.FishingClubs.FirstOrDefaultAsync(fc => fc.Id == fishingClubId);
        if (fishingClub == null) return null;
        _mapper.Map(fishingClubDto, fishingClub);
        await _context.SaveChangesAsync();
        return _mapper.Map<FishingClubDto>(fishingClub);
    }

    public async Task<bool> DeleteFishingClubAsync(Guid fishingClubId)
    {
        var fishingClub = await _context.FishingClubs.FirstOrDefaultAsync(fc => fc.Id == fishingClubId);
        if (fishingClub == null) return false;
        _context.FishingClubs.Remove(fishingClub);
        await _context.SaveChangesAsync();
        return true;
    }
}