using Application.DataTransferObjects.CatchDetail;

namespace Application.Interfaces;

public interface ICatchDetailRepository
{
    Task<CatchDetailDto> GetCatchDetailAsync(Guid catchDetailId);
    Task<List<CatchDetailDto>> GetCatchDetailsByCatchId(Guid catchId);
    Task<CatchDetailDto> CreateCatchDetailAsync(CreateCatchDetailDto createCatchDetailDto);
    Task<CatchDetailDto> UpdateCatchDetailAsync(UpdateCatchDetailDto updateCatchDetailDto);
    Task<bool> DeleteCatchDetailAsync(Guid catchDetailId);
}