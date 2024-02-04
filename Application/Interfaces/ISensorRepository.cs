using Application.DataTransferObjects.Sensor;

namespace Application.Interfaces;

// Interface for managing sensor-related operations
public interface ISensorRepository
{
    // Get a list of all sensors
    Task<List<SensorDto>> GetSensorsAsync();

    // Get a specific sensor by its ID
    Task<SensorDto> GetSensorByIdAsync(Guid sensorId);

    // Get a specific sensor by its DevEui
    Task<SensorDto> GetSensorByDevEuiAsync(string devEui);

    // Create a new sensor with the specified details
    Task<SensorDto> CreatorSensorAsync(CreateSensorDto createSensorDto);

    // Update an existing sensor by its ID
    Task<SensorDto> UpdateSensorAsync(UpdateSensorDto updateSensorDto);

    // Delete a sensor by its ID
    Task<bool> DeleteSensorAsync(Guid sensorId);
}