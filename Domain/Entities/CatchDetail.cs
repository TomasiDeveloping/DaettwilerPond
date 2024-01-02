namespace Domain.Entities;

public class CatchDetail
{
    public Guid Id { get; set; }
    public Guid CatchId { get; set; }
    public Catch Catch { get; set; }
    public Guid FishTypeId { get; set; }
    public FishType FishType { get; set; }
    public int Amount { get; set; }
    public bool HadCrabs { get; set; }
}