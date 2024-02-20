using Application.DataTransferObjects.FishingLicense;
using Application.DataTransferObjects.Overseer;
using Application.Interfaces;
using Application.Models.CatchReport;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

// FishingLicenseRepository handles CRUD operations for FishingLicenses
public class FishingLicenseRepository(DaettwilerPondDbContext context, IMapper mapper) : IFishingLicenseRepository
{

    // Get a list of all fishing licenses, ordered by descending year
    public async Task<List<FishingLicenseDto>> GetFishingLicensesAsync()
    {
        // Projecting and querying a list of FishingLicenseDto
        var licenses = await context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(l => l.Year)
            .ToListAsync();
        return licenses;
    }

    // Get a specific fishing license by ID
    public async Task<FishingLicenseDto> GetFishingLicenseAsync(Guid fishingLicenseId)
    {
        // Projecting and querying a specific FishingLicenseDto by ID
        var license = await context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == fishingLicenseId);
        return license;
    }

    // Get fishing licenses associated with a specific user, ordered by descending year
    public async Task<List<FishingLicenseDto>> GetUserFishingLicenses(Guid userId)
    {
        // Projecting and querying a list of FishingLicenseDto for a specific user
        var userLicenses = await context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(l => l.Year)
            .Where(l => l.UserId == userId)
            .ToListAsync();
        return userLicenses;
    }

    // Get the fishing license for the current year associated with a specific user
    public async Task<FishingLicenseDto> GetUserFishingLicenseForCurrentYear(Guid userId)
    {
        // Projecting and querying the FishingLicenseDto for the current year and user
        var currentUserLicense = await context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(l => l.UserId == userId && l.Year == DateTime.Now.Year)
            .FirstOrDefaultAsync();
        return currentUserLicense;
    }

    public async Task<CatchDetailsYearDto> GetDetailYearlyCatchAsync(int year)
    {
        var catchDetails = await context.FishingLicenses
            .Where(l => l.Year == year)
            .SelectMany(l => l.Catches)
            .GroupBy(c => 1) // Group by a constant to aggregate all records into a single group
            .Select(g => new CatchDetailsYearDto
            {
                TotalFishCatches = g.SelectMany(c => c.CatchDetails).Sum(cd => cd.Amount),
                TotalHoursSpend = g.Sum(c => c.HoursSpent),
                CurrentYear = year
            })
            .FirstOrDefaultAsync();

        return catchDetails;
    }

    public async Task<List<UserStatistic>> GetDetailYearlyCatchReportAsync(int year)
    {
        // Fetch all fishing licenses for the given year
        var userStatistic = await context.FishingLicenses
            .Where(fl => fl.Year == year)
            .ProjectTo<UserStatistic>(mapper.ConfigurationProvider)
            .ToListAsync();

        return userStatistic;
    }

    public async Task<UserStatistic> GetYearlyUserCatchReportAsync(Guid userId, int year)
    {
        var userStatistic = await context.FishingLicenses
            .Where(l => l.UserId == userId && l.Year == year)
            .ProjectTo<UserStatistic>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return userStatistic;
    }

    public async Task<OverseerMemberDetailsDto> GetOverseerMemberDetailAsync(Guid userId)
    {
        var today = DateTime.Now;
        var currentYear = today.Year;
        
        var memberDetail = await context.FishingLicenses
            .Include(l => l.User)
            .Include(l => l.Catches).ThenInclude(c => c.CatchDetails)
            .Where(l => l.User.Id == userId && l.Year == currentYear)
            .Select(l => new OverseerMemberDetailsDto()
            {
                UserFullName = $"{l.User.FirstName} {l.User.LastName}",
                IsLicencePaid = l.IsPaid,
                IsLicenceActive = l.IsActive,
                LicenseIssuedBy = l.IssuedBy,
                LicenseIssuedOn = l.CreatedAt,
                LicenseValidUntil = l.ExpiresOn,
                IsFishing = l.Catches.Any(c => c.StartFishing.HasValue && !c.EndFishing.HasValue && c.StartFishing.Value.Date == today.Date),
                TotalFishes = l.Catches.SelectMany(d => d.CatchDetails).Count(),
                TotalHours = l.Catches.Sum(c => c.HoursSpent)
            })
            .FirstOrDefaultAsync();

        return memberDetail;
    }

    // Create a new fishing license
    public async Task<FishingLicenseDto> CreateFishingLicenseAsync(CreateFishingLicenseDto createFishingLicenseDto,
        string issuer)
    {
        // Mapping and adding a new FishingLicense to the database
        var license = mapper.Map<FishingLicense>(createFishingLicenseDto);
        license.IssuedBy = issuer;

        // Setting expiration time to the end of the day
        license.ExpiresOn = license.ExpiresOn.AddDays(1).AddSeconds(-1);

        // Adding the new license to the database and saving changes
        await context.FishingLicenses.AddAsync(license);
        await context.SaveChangesAsync();

        // Returning the newly created FishingLicense
        return await GetFishingLicenseAsync(license.Id);
    }

    // Update an existing fishing license
    public async Task<FishingLicenseDto> UpdateFishingLicenseAsync(FishingLicenseDto fishingLicenseDto)
    {
        // Finding the fishing license to update
        var license = await context.FishingLicenses.FirstOrDefaultAsync(l => l.Id == fishingLicenseDto.Id);
        if (license == null) return null;

        // Mapping and updating the fishing license
        mapper.Map(fishingLicenseDto, license);

        // Updating the last modified timestamp and saving changes
        license.UpdatedAt = DateTime.Now;
        await context.SaveChangesAsync();

        // Returning the updated FishingLicense
        return await GetFishingLicenseAsync(license.Id);
    }

    // Delete a fishing license by ID
    public async Task<bool> DeleteFishingLicenseAsync(Guid fishingLicenseId)
    {
        // Finding the fishing license to delete
        var license = await context.FishingLicenses.FirstOrDefaultAsync(l => l.Id == fishingLicenseId);
        if (license == null) return false;

        // Removing the fishing license and saving changes
        context.FishingLicenses.Remove(license);
        await context.SaveChangesAsync();

        // Returning true to indicate successful deletion
        return true;
    }
}