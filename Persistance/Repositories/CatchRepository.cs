using Application.DataTransferObjects.Catch;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CatchRepository(DaettwilerPondDbContext context, IMapper mapper) : ICatchRepository
{
    public async Task<List<CatchDto>> GetCatchesByLicenceIdAsync(Guid licenceId)
    {
        var catches = await context.Catches
            .ProjectTo<CatchDto>(mapper.ConfigurationProvider)
            .Where(c => c.FishingLicenseId == licenceId)
            .ToListAsync();
        return catches;
    }

    public async Task<CatchDto> GetCatchAsync(Guid catchId)
    {
        var fishCatch = await context.Catches
            .ProjectTo<CatchDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id == catchId);
        return fishCatch;
    }

    public async Task<CatchDto> UpdateCatchAsync(UpdateCatchDto updateCatchDto)
    {
        var catchToUpdate = await context.Catches.FirstOrDefaultAsync(c => c.Id == updateCatchDto.Id);
        if (catchToUpdate is null) return null;
        mapper.Map(updateCatchDto, catchToUpdate);
        await context.SaveChangesAsync();
        return mapper.Map<CatchDto>(catchToUpdate);
    }

    public async Task<CatchDto> CreateCatchAsync(CreateCatchDto createCatchDto)
    {
        var newCatch = mapper.Map<Catch>(createCatchDto);
        await context.Catches.AddAsync(newCatch);
        await context.SaveChangesAsync();
        return mapper.Map<CatchDto>(newCatch);
    }

    public async Task<bool> DeleteCatchAsync(Guid catchId)
    {
        var catchToDelete = await context.Catches.FirstOrDefaultAsync(c => c.Id == catchId);
        if (catchToDelete is null) return false;
        context.Catches.Remove(catchToDelete);
        await context.SaveChangesAsync();
        return true;
    }
}