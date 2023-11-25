using Application.DataTransferObjects.FishingLicense;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FishingLicenseRepository(DaettwilerPondDbContext context, IMapper mapper) : IFishingLicenseRepository
{
    public async Task<List<FishingLicenseDto>> GetFishingLicensesAsync()
    {
        var licenses = await context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(l => l.Year)
            .ToListAsync();
        return licenses;
    }

    public async Task<FishingLicenseDto> GetFishingLicenseAsync(Guid fishingLicenseId)
    {
        var license = await context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == fishingLicenseId);
        return license;
    }

    public async Task<List<FishingLicenseDto>> GetUserFishingLicenses(Guid userId)
    {
        var userLicenses = await context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(l => l.Year)
            .Where(l => l.UserId == userId)
            .ToListAsync();
        return userLicenses;
    }

    public async Task<FishingLicenseDto> GetUserFishingLicenseForCurrentYear(Guid userId)
    {
        var currentUserLicense = await context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(l => l.UserId == userId && l.Year == DateTime.Now.Year)
            .FirstOrDefaultAsync();
        return currentUserLicense;
    }

    public async Task<FishingLicenseDto> CreateFishingLicenseAsync(CreateFishingLicenseDto createFishingLicenseDto,
        string issuer)
    {
        var license = mapper.Map<FishingLicense>(createFishingLicenseDto);
        license.IssuedBy = issuer;
        license.ExpiresOn = license.ExpiresOn.AddDays(1).AddSeconds(-1);
        await context.FishingLicenses.AddAsync(license);
        await context.SaveChangesAsync();
        return await GetFishingLicenseAsync(license.Id);
    }

    public async Task<FishingLicenseDto> UpdateFishingLicenseAsync(FishingLicenseDto fishingLicenseDto)
    {
        var license = await context.FishingLicenses.FirstOrDefaultAsync(l => l.Id == fishingLicenseDto.Id);
        if (license == null) return null;
        mapper.Map(fishingLicenseDto, license);
        license.UpdatedAt = DateTime.Now;
        await context.SaveChangesAsync();
        return await GetFishingLicenseAsync(license.Id);
    }

    public async Task<bool> DeleteFishingLicenseAsync(Guid fishingLicenseId)
    {
        var license = await context.FishingLicenses.FirstOrDefaultAsync(l => l.Id == fishingLicenseId);
        if (license == null) return false;
        context.FishingLicenses.Remove(license);
        await context.SaveChangesAsync();
        return true;
    }
}