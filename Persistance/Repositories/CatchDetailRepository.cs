using Application.DataTransferObjects.CatchDetail;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CatchDetailRepository(DaettwilerPondDbContext context, IMapper mapper) : ICatchDetailRepository
{
    public async Task<CatchDetailDto> GetCatchDetailAsync(Guid catchDetailId)
    {
        var catchDetail = await context.CatchDetails
            .ProjectTo<CatchDetailDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cd => cd.Id == catchDetailId);
        return catchDetail;
    }

    public async Task<List<CatchDetailDto>> GetCatchDetailsByCatchId(Guid catchId)
    {
        var catchDetails = await context.CatchDetails
            .ProjectTo<CatchDetailDto>(mapper.ConfigurationProvider)
            .Where(cd => cd.CatchId == catchId)
            .ToListAsync();
        return catchDetails;
    }

    public async Task<CatchDetailDto> CreateCatchDetailAsync(CreateCatchDetailDto createCatchDetailDto)
    {
        var newCatchDetail = mapper.Map<CatchDetail>(createCatchDetailDto);
        await context.CatchDetails.AddAsync(newCatchDetail);
        await context.SaveChangesAsync();
        return mapper.Map<CatchDetailDto>(newCatchDetail);
    }

    public async Task<CatchDetailDto> UpdateCatchDetailAsync(UpdateCatchDetailDto updateCatchDetailDto)
    {
        var catchDetailToUpdate =
            await context.CatchDetails.FirstOrDefaultAsync(cd => cd.Id == updateCatchDetailDto.Id);
        if (catchDetailToUpdate is null) return null;
        mapper.Map(updateCatchDetailDto, catchDetailToUpdate);
        await context.SaveChangesAsync();
        return mapper.Map<CatchDetailDto>(catchDetailToUpdate);
    }

    public async Task<bool> DeleteCatchDetailAsync(Guid catchDetailId)
    {
        var catchDetailToDelete = await context.CatchDetails.FirstOrDefaultAsync(c => c.Id == catchDetailId);
        if (catchDetailToDelete is null) return false;
        context.CatchDetails.Remove(catchDetailToDelete);
        await context.SaveChangesAsync();
        return true;
    }
}