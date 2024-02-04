using Application.DataTransferObjects.FishingRegulation;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

// FishingRegulationRepository handles CRUD operations for FishingRegulations
public class FishingRegulationRepository(DaettwilerPondDbContext context, IMapper mapper) : IFishingRegulationRepository
{

    // Get a list of all fishing regulations
    public async Task<List<FishingRegulationDto>> GetFishingRegulationsAsync()
    {
        // Projecting and querying a list of FishingRegulationDto
        var fishingRegulations = await context.FishingRegulations
            .ProjectTo<FishingRegulationDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();
        return fishingRegulations;
    }


    // Create a new fishing regulation
    public async Task<FishingRegulationDto> CreateFishingRegulationAsync(CreateFishingRegulationDto createFishingRegulationDto)
    {
        // Mapping and adding a new FishingRegulation to the database
        var fishingRegulation = mapper.Map<FishingRegulation>(createFishingRegulationDto);
        await context.FishingRegulations.AddAsync(fishingRegulation);
        await context.SaveChangesAsync();
        return mapper.Map<FishingRegulationDto>(fishingRegulation);
    }

    // Update an existing fishing regulation
    public async Task<FishingRegulationDto> UpdateFishingRegulationAsync(Guid fishingRegulationId,
        FishingRegulationDto fishingRegulationDto)
    {
        // Finding the fishing regulation to update
        var fishingRegulation =
            await context.FishingRegulations.FirstOrDefaultAsync(fr => fr.Id == fishingRegulationId);
        if (fishingRegulation == null) return null;

        // Mapping and updating the fishing regulation
        mapper.Map(fishingRegulationDto, fishingRegulation);
        await context.SaveChangesAsync();
        return mapper.Map<FishingRegulationDto>(fishingRegulation);
    }

    // Delete a fishing regulation by ID
    public async Task<bool> DeleteFishingRegulationAsync(Guid fishingRegulationId)
    {
        // Finding the fishing regulation to delete
        var fishingRegulation =
            await context.FishingRegulations.FirstOrDefaultAsync(fr => fr.Id == fishingRegulationId);
        if (fishingRegulation == null) return false;

        // Removing the fishing regulation and saving changes
        context.FishingRegulations.Remove(fishingRegulation);
        await context.SaveChangesAsync();

        // Returning true to indicate successful deletion
        return true;
    }
}