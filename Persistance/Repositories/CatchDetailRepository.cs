using Application.DataTransferObjects.CatchDetail;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;


// CatchDetailRepository handles CRUD operations for CatchDetails
public class CatchDetailRepository(DaettwilerPondDbContext context, IMapper mapper) : ICatchDetailRepository
{

    // Get a specific catch detail by ID
    public async Task<CatchDetailDto> GetCatchDetailAsync(Guid catchDetailId)
    {
        // Projecting and querying CatchDetailDto from the database
        var catchDetail = await context.CatchDetails
            .ProjectTo<CatchDetailDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cd => cd.Id == catchDetailId);
        return catchDetail;
    }

    // Get all catch details for a specific catch
    public async Task<List<CatchDetailDto>> GetCatchDetailsByCatchId(Guid catchId)
    {
        // Projecting and querying a list of CatchDetailDto for a specific catch
        var catchDetails = await context.CatchDetails
            .ProjectTo<CatchDetailDto>(mapper.ConfigurationProvider)
            .Where(cd => cd.CatchId == catchId)
            .ToListAsync();
        return catchDetails;
    }

    // Create a new catch detail
    public async Task<CatchDetailDto> CreateCatchDetailAsync(CreateCatchDetailDto createCatchDetailDto)
    {
        // Mapping and adding a new CatchDetail to the database
        var newCatchDetail = mapper.Map<CatchDetail>(createCatchDetailDto);
        await context.CatchDetails.AddAsync(newCatchDetail);
        await context.SaveChangesAsync();
        return mapper.Map<CatchDetailDto>(newCatchDetail);
    }

    // Update an existing catch detail
    public async Task<CatchDetailDto> UpdateCatchDetailAsync(UpdateCatchDetailDto updateCatchDetailDto)
    {
        // Finding the catch detail to update
        var catchDetailToUpdate =
            await context.CatchDetails.FirstOrDefaultAsync(cd => cd.Id == updateCatchDetailDto.Id);
        if (catchDetailToUpdate is null) return null;

        // Mapping and updating the catch detail
        mapper.Map(updateCatchDetailDto, catchDetailToUpdate);
        await context.SaveChangesAsync();
        return mapper.Map<CatchDetailDto>(catchDetailToUpdate);
    }

    // Delete a catch detail by ID
    public async Task<bool> DeleteCatchDetailAsync(Guid catchDetailId)
    {
        // Finding the catch detail to delete
        var catchDetailToDelete = await context.CatchDetails.FirstOrDefaultAsync(c => c.Id == catchDetailId);
        if (catchDetailToDelete is null) return false;

        // Removing the catch detail and saving changes
        context.CatchDetails.Remove(catchDetailToDelete);
        await context.SaveChangesAsync();
        return true;
    }
}