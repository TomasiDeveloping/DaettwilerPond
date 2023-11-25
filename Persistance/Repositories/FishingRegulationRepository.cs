using Application.DataTransferObjects.FishingRegulation;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FishingRegulationRepository(DaettwilerPondDbContext context, IMapper mapper) : IFishingRegulationRepository
{
    public async Task<List<FishingRegulationDto>> GetFishingRegulationsAsync()
    {
        var fishingRegulations = await context.FishingRegulations
            .ProjectTo<FishingRegulationDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return fishingRegulations;
    }

    public async Task<FishingRegulationDto> CreateFishingRegulationAsync(CreateFishingRegulationDto createFishingRegulationDto)
    {
        var fishingRegulation = mapper.Map<FishingRegulation>(createFishingRegulationDto);
        await context.FishingRegulations.AddAsync(fishingRegulation);
        await context.SaveChangesAsync();
        return mapper.Map<FishingRegulationDto>(fishingRegulation);
    }

    public async Task<FishingRegulationDto> UpdateFishingRegulationAsync(Guid fishingRegulationId,
        FishingRegulationDto fishingRegulationDto)
    {
        var fishingRegulation =
            await context.FishingRegulations.FirstOrDefaultAsync(fr => fr.Id == fishingRegulationId);
        if (fishingRegulation == null) return null;
        mapper.Map(fishingRegulationDto, fishingRegulation);
        await context.SaveChangesAsync();
        return mapper.Map<FishingRegulationDto>(fishingRegulation);
    }

    public async Task<bool> DeleteFishingRegulationAsync(Guid fishingRegulationId)
    {
        var fishingRegulation =
            await context.FishingRegulations.FirstOrDefaultAsync(fr => fr.Id == fishingRegulationId);
        if (fishingRegulation == null) return false;
        context.FishingRegulations.Remove(fishingRegulation);
        await context.SaveChangesAsync();
        return true;
    }
}