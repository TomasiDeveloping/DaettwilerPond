using Application.DataTransferObjects.FishingRegulation;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FishingRegulationRepository : IFishingRegulationRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;

    public FishingRegulationRepository(DaettwilerPondDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FishingRegulationDto>> GetFishingRegulationsAsync()
    {
        var fishingRegulations = await _context.FishingRegulations
            .ProjectTo<FishingRegulationDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return fishingRegulations;
    }

    public async Task<FishingRegulationDto> CreateFishingRegulationAsync(FishingRegulationDto fishingRegulationDto)
    {
        var fishingRegulation = _mapper.Map<FishingRegulation>(fishingRegulationDto);
        fishingRegulation.Id = Guid.NewGuid();
        await _context.FishingRegulations.AddAsync(fishingRegulation);
        await _context.SaveChangesAsync();
        return _mapper.Map<FishingRegulationDto>(fishingRegulation);
    }

    public async Task<FishingRegulationDto> UpdateFishingRegulationAsync(Guid fishingRegulationId,
        FishingRegulationDto fishingRegulationDto)
    {
        var fishingRegulation =
            await _context.FishingRegulations.FirstOrDefaultAsync(fr => fr.Id == fishingRegulationId);
        if (fishingRegulation == null) return null;
        _mapper.Map(fishingRegulationDto, fishingRegulation);
        await _context.SaveChangesAsync();
        return _mapper.Map<FishingRegulationDto>(fishingRegulation);
    }

    public async Task<bool> DeleteFishingRegulationAsync(Guid fishingRegulationId)
    {
        var fishingRegulation =
            await _context.FishingRegulations.FirstOrDefaultAsync(fr => fr.Id == fishingRegulationId);
        if (fishingRegulation == null) return false;
        _context.FishingRegulations.Remove(fishingRegulation);
        await _context.SaveChangesAsync();
        return true;
    }
}