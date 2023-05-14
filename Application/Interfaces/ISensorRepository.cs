using Application.DataTransferObjects.Sensor;

namespace Application.Interfaces;

public interface ISensorRepository
{
    Task<List<SensorDto>> GetSensorsAsync();
    Task<SensorDto> GetSensorByIdAsync(Guid sensorId);
    Task<SensorDto> GetSensorByDevEuiAsync(string devEui);
    Task<SensorDto> CreatorSensorAsync(CreateSensorDto createSensorDto);
    Task<SensorDto> UpdateSensorAsync(UpdateSensorDto updateSensorDto);
    Task<bool> DeleteSensorAsync(Guid sensorId);
}