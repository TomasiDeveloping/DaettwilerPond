using Application.DataTransferObjects.SensorType;

namespace Application.Interfaces;

public interface ISensorTypeRepository
{
    Task<List<SensorTypeDto>> GetSensorTypesAsync();
    Task<SensorTypeDto> GetSensorTypeByIdAsync(Guid sensorTypeId);
    Task<SensorTypeDto> CreateSensorTypeAsync(CreateSensorTypeDto createSensorTypeDto);
    Task<SensorTypeDto> UpdateSensorTypeAsync(SensorTypeDto sensorTypeDto);
    Task<bool> DeleteSensorTypeAsync(Guid sensorTypeId);
}