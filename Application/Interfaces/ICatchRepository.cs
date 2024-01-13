using Application.DataTransferObjects.Catch;

namespace Application.Interfaces;

public interface ICatchRepository
{
    Task<List<CatchDto>> GetCatchesByLicenceIdAsync(Guid licenceId);
    Task<bool> CheckCatchDateExistsAsync(Guid licenceId, DateTime catchDate);
    Task<CatchDto> GetCatchAsync(Guid catchId);
    Task<CatchDto> GetCatchForCurrentDayAsync(Guid licenceId);
    Task<YearlyCatch> GetYearlyCatchAsync(Guid licenceId);
    Task<CatchDto> UpdateCatchAsync(UpdateCatchDto updateCatchDto);
    Task<CatchDto> CreateCatchAsync(CreateCatchDto createCatchDto);
    Task<CatchDto> StartCatchDayAsync(Guid licenceId);
    Task<CatchDto> StopCatchDayAsync(Guid catchId);
    Task<CatchDto> ContinueFishingDayAsync(Guid catchId);
    Task<bool> DeleteCatchAsync(Guid catchId);
}