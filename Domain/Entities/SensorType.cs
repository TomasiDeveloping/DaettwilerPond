namespace Domain.Entities;

public class SensorType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Producer { get; set; }
    public ICollection<Sensor> Sensors { get; set; }
}