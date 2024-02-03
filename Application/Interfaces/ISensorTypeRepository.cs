using Application.DataTransferObjects.SensorType;

namespace Application.Interfaces;

// Interface for managing sensor type-related operations
public interface ISensorTypeRepository
{
    // Get a list of all sensor types
    Task<List<SensorTypeDto>> GetSensorTypesAsync();

    // Get a specific sensor type by its ID
    Task<SensorTypeDto> GetSensorTypeByIdAsync(Guid sensorTypeId);

    // Create a new sensor type with the specified details
    Task<SensorTypeDto> CreateSensorTypeAsync(CreateSensorTypeDto createSensorTypeDto);

    // Update an existing sensor type by its ID
    Task<SensorTypeDto> UpdateSensorTypeAsync(SensorTypeDto sensorTypeDto);

    // Delete a sensor type by its ID
    Task<bool> DeleteSensorTypeAsync(Guid sensorTypeId);
}