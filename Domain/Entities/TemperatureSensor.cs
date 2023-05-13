namespace Domain.Entities;

public class TemperatureSensor
{
    public Guid Id { get; set; }
    public string DevEui { get; set; }
    public string ApplicationId { get; set; }
    public string Payload { get; set; }
    public decimal Battery { get; set; }
    public decimal Temperature { get; set; }
    public DateTime ReceivedAt { get; set; }
}