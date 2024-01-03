using Application.DataTransferObjects.Catch;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Helpers;

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
        if (updateCatchDto.StartFishing.HasValue && updateCatchDto.EndFishing.HasValue)
        {
            catchToUpdate.HoursSpent =
                CalculateHoursSpent(updateCatchDto.StartFishing.Value, updateCatchDto.EndFishing.Value);
        }
        await context.SaveChangesAsync();
        return mapper.Map<CatchDto>(catchToUpdate);
    }

    public async Task<CatchDto> CreateCatchAsync(CreateCatchDto createCatchDto)
    {
        var newCatch = mapper.Map<Catch>(createCatchDto);
        if (createCatchDto.StartFishing.HasValue && createCatchDto.EndFishing.HasValue)
        {
            createCatchDto.HoursSpent =
                CalculateHoursSpent(createCatchDto.StartFishing.Value, createCatchDto.EndFishing.Value);
        }
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

    private static double CalculateHoursSpent(DateTime startDate, DateTime endDate)
    {
        var different = endDate.Subtract(startDate);
        var differentToNearest15Minutes = different.RoundToNearestMinutes(15);
        return Math.Round(differentToNearest15Minutes.TotalHours, 2);
    }
}