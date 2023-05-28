using Application.DataTransferObjects.FishingLicense;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FishingLicenseRepository : IFishingLicenseRepository
{
    private readonly DaettwilerPondDbContext _context;
    private readonly IMapper _mapper;

    public FishingLicenseRepository(DaettwilerPondDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FishingLicenseDto>> GetFishingLicensesAsync()
    {
        var licenses = await _context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(l => l.Year)
            .ToListAsync();
        return licenses;
    }

    public async Task<FishingLicenseDto> GetFishingLicenseAsync(Guid fishingLicenseId)
    {
        var license = await _context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == fishingLicenseId);
        return license;

    }

    public async Task<List<FishingLicenseDto>> GetUserFishingLicenses(Guid userId)
    {
        var userLicenses = await _context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .OrderByDescending(l => l.Year)
            .Where(l => l.UserId == userId)
            .ToListAsync();
        return userLicenses;
    }

    public async Task<FishingLicenseDto> GetUserFishingLicenseForCurrentYear(Guid userId)
    {
        var currentUserLicense = await _context.FishingLicenses
            .ProjectTo<FishingLicenseDto>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .Where(l => l.UserId == userId && l.Year == DateTime.Now.Year)
            .FirstOrDefaultAsync();
        return currentUserLicense;
    }

    public async Task<FishingLicenseDto> CreateFishingLicenseAsync(CreateFishingLicenseDto createFishingLicenseDto,
        string issuer)
    {
        var license = _mapper.Map<FishingLicense>(createFishingLicenseDto);
        license.IssuedBy = issuer;
        license.ExpiresOn = license.ExpiresOn.AddDays(1).AddSeconds(-1);
        await _context.FishingLicenses.AddAsync(license);
        await _context.SaveChangesAsync();
        return await GetFishingLicenseAsync(license.Id);
    }

    public async Task<FishingLicenseDto> UpdateFishingLicenseAsync(FishingLicenseDto fishingLicenseDto)
    {
        var license = await _context.FishingLicenses.FirstOrDefaultAsync(l => l.Id == fishingLicenseDto.Id);
        if (license == null) return null;
        _mapper.Map(fishingLicenseDto, license);
        license.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return await GetFishingLicenseAsync(license.Id);
    }

    public async Task<bool> DeleteFishingLicenseAsync(Guid fishingLicenseId)
    {
        var license = await _context.FishingLicenses.FirstOrDefaultAsync(l => l.Id == fishingLicenseId);
        if (license == null) return false;
        _context.FishingLicenses.Remove(license);
        await _context.SaveChangesAsync();
        return true;
    }
}