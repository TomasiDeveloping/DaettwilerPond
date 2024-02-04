using Application.DataTransferObjects.Catch;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Helpers;

namespace Persistence.Repositories;

// CatchRepository handles CRUD operations for Catches
public class CatchRepository(DaettwilerPondDbContext context, IMapper mapper) : ICatchRepository
{

    // Get all catches for a specific fishing license
    public async Task<List<CatchDto>> GetCatchesByLicenceIdAsync(Guid licenceId)
    {
        // Projecting and querying a list of CatchDto for a specific fishing license
        var catches = await context.Catches
            .ProjectTo<CatchDto>(mapper.ConfigurationProvider)
            .Where(c => c.FishingLicenseId == licenceId)
            .ToListAsync();
        return catches;
    }

    // Check if a catch for a specific date exists
    public async Task<bool> CheckCatchDateExistsAsync(Guid licenceId, DateTime catchDate)
    {
        // Checking if a catch exists for a specific date
        var catchDay = await context.Catches
            .AsNoTracking()
            .FirstOrDefaultAsync(c =>
                c.FishingLicenseId == licenceId && c.CatchDate.Year == catchDate.Year &&
                c.CatchDate.Month == catchDate.Month && c.CatchDate.Day == catchDate.Day);
        return catchDay != null;
    }

    // Get a specific catch by ID
    public async Task<CatchDto> GetCatchAsync(Guid catchId)
    {
        // Projecting and querying a specific CatchDto by ID
        var fishCatch = await context.Catches
            .ProjectTo<CatchDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id == catchId);
        return fishCatch;
    }

    // Get all catches for a specific month of a fishing license
    public async Task<List<CatchDto>> GetCatchesForMonthAsync(Guid licenceId, int month)
    {
        // Projecting and querying a list of CatchDto for a specific month
        var monthCatches = await context.Catches
            .ProjectTo<CatchDto>(mapper.ConfigurationProvider)
            .Where(c => c.FishingLicenseId == licenceId && c.CatchDate.Month == month)
            .OrderBy(c => c.CatchDate)
            .ToListAsync();
        return monthCatches;
    }

    // Get the catch for the current day of a fishing license
    public async Task<CatchDto> GetCatchForCurrentDayAsync(Guid licenceId)
    {
        // Projecting and querying the catch for the current day
        var date = DateTime.Now;
        var fishCatch = await context.Catches
            .ProjectTo<CatchDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c =>
                c.FishingLicenseId == licenceId && c.CatchDate.Year == date.Year && c.CatchDate.Month == date.Month &&
                c.CatchDate.Day == date.Day);
        return fishCatch;
    }

    // Get yearly catch statistics for a fishing license
    public async Task<YearlyCatch> GetYearlyCatchAsync(Guid licenceId)
    {
        // Projecting and querying yearly catch statistics
        var currentYear = DateTime.Now.Year;
        var yearlyCatch = await context.FishingLicenses
            .Include(l => l.Catches)
            .ThenInclude(c => c.CatchDetails)
            .Where(l => l.Id == licenceId && l.Year == currentYear)
            .Select(x => new YearlyCatch()
            {
                HoursSpent = x.Catches.Sum(c => c.HoursSpent),
                FishCatches = x.Catches.SelectMany(d => d.CatchDetails).Count()
            })
            .FirstOrDefaultAsync();
        return yearlyCatch;
    }

    // Get detailed yearly catch statistics for a fishing license
    public async Task<List<DetailYearlyCatch>> GetDetailYearlyCatchAsync(Guid licenceId)
    {
        // Projecting and querying detailed yearly catch statistics
        var detailCatches = await context.Catches
            .Include(c => c.CatchDetails)
            .Where(c => c.FishingLicenseId == licenceId)
            .GroupBy(x => x.CatchDate.Month)
            .Select(group => new DetailYearlyCatch()
            {
                Month = group.Key,
                HoursSpent = group.Sum(c => c.HoursSpent),
                Fishes = group.SelectMany(c => c.CatchDetails).Count()
            })
            .ToListAsync();
        return detailCatches;
    }

    // Update an existing catch
    public async Task<CatchDto> UpdateCatchAsync(UpdateCatchDto updateCatchDto)
    {
        // Finding the catch to update
        var catchToUpdate = await context.Catches.FirstOrDefaultAsync(c => c.Id == updateCatchDto.Id);
        if (catchToUpdate is null) return null;

        // Mapping and updating the catch
        mapper.Map(updateCatchDto, catchToUpdate);

        // Calculating and updating hours spent if start and end fishing times are provided
        if (updateCatchDto.StartFishing.HasValue && updateCatchDto.EndFishing.HasValue)
        {
            catchToUpdate.HoursSpent =
                CalculateHoursSpent(updateCatchDto.StartFishing.Value, updateCatchDto.EndFishing.Value);
        }

        // Saving changes and returning the updated catch
        await context.SaveChangesAsync();
        return mapper.Map<CatchDto>(catchToUpdate);
    }

    // Create a new catch
    public async Task<CatchDto> CreateCatchAsync(CreateCatchDto createCatchDto)
    {
        // Mapping and adding a new catch to the database
        var newCatch = mapper.Map<Catch>(createCatchDto);

        // Calculating and setting hours spent if start and end fishing times are provided
        if (createCatchDto.StartFishing.HasValue && createCatchDto.EndFishing.HasValue)
        {
            createCatchDto.HoursSpent =
                CalculateHoursSpent(createCatchDto.StartFishing.Value, createCatchDto.EndFishing.Value);
        }

        // Adding the new catch to the database and saving changes
        await context.Catches.AddAsync(newCatch);
        await context.SaveChangesAsync();

        // Returning the newly created catch
        return mapper.Map<CatchDto>(newCatch);
    }

    // Start a new catch day
    public async Task<CatchDto> StartCatchDayAsync(Guid licenceId)
    {
        // Creating and adding a new catch for the current day to the database
        var newCatch = new Catch()
        {
            CatchDate = DateTime.Now,
            FishingLicenseId = licenceId,
            HoursSpent = 0,
            StartFishing = DateTime.Now,
        };
        await context.Catches.AddAsync(newCatch);
        await context.SaveChangesAsync();

        // Returning the newly created catch
        return mapper.Map<CatchDto>(newCatch);
    }

    // Stop the catch day and calculate hours spent
    public async Task<CatchDto> StopCatchDayAsync(Guid catchId, DateTime endTime = default)
    {
        // Finding the catch to stop
        var currentCatch = await context.Catches.FirstOrDefaultAsync(c => c.Id == catchId);
        if (currentCatch is null) return null;

        // Checking if the catch has a start date
        if (!currentCatch.StartFishing.HasValue) throw new ArgumentException("No Start date");

        // Setting end fishing time, calculating hours spent, and saving changes
        if (endTime == default) endTime = DateTime.Now;
        currentCatch.EndFishing = endTime;
        currentCatch.HoursSpent += CalculateHoursSpent(currentCatch.StartFishing.Value, endTime);
        await context.SaveChangesAsync();

        // Returning the updated catch
        return mapper.Map<CatchDto>(currentCatch);
    }

    // Continue fishing on the current day
    public async Task<CatchDto> ContinueFishingDayAsync(Guid catchId)
    {
        // Finding the catch to continue
        var currentCatch = await context.Catches.FirstOrDefaultAsync(c => c.Id == catchId);
        if (currentCatch is null) return null;

        // Checking if the catch is on the same day as the current date
        var currentDate = DateTime.Now;
        if (currentCatch.CatchDate.Year != currentDate.Year ||
            currentCatch.CatchDate.Day != currentDate.Day) return null;

        // Setting start fishing time and end fishing time to null, and saving changes
        currentCatch.StartFishing = currentDate;
        currentCatch.EndFishing = null;
        await context.SaveChangesAsync();

        // Returning the updated catch
        return mapper.Map<CatchDto>(currentCatch);
    }

    // Delete a catch and its associated catch details
    public async Task<bool> DeleteCatchAsync(Guid catchId)
    {
        // Finding the catch to delete
        var catchToDelete = await context.Catches
            .Include (c => c.CatchDetails)
            .FirstOrDefaultAsync(c => c.Id == catchId);
        if (catchToDelete is null) return false;

        // Removing associated catch details if any
        if (catchToDelete.CatchDetails.Any())
        {
            context.CatchDetails.RemoveRange(catchToDelete.CatchDetails);
        }

        // Removing the catch and saving changes
        context.Catches.Remove(catchToDelete);
        await context.SaveChangesAsync();

        // Returning true to indicate successful deletion
        return true;
    }

    // Calculate hours spent based on start and end fishing times
    private static double CalculateHoursSpent(DateTime startDate, DateTime endDate)
    {
        // Calculating and rounding hours spent to the nearest 15 minutes
        var different = endDate.Subtract(startDate);
        var differentToNearest15Minutes = different.RoundToNearestMinutes(15);
        return Math.Round(differentToNearest15Minutes.TotalHours, 2);
    }
}