using Application.DataTransferObjects.Catch;

namespace Application.Interfaces;

public interface ICatchRepository
{
    Task<List<CatchDto>> GetCatchesByLicenceIdAsync(Guid licenceId);
    Task<CatchDto> GetCatchAsync(Guid catchId);
    Task<CatchDto> UpdateCatchAsync(UpdateCatchDto updateCatchDto);
    Task<CatchDto> CreateCatchAsync(CreateCatchDto createCatchDto);
    Task<bool> DeleteCatchAsync(Guid catchId);
}