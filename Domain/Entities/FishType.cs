namespace Domain.Entities;

public class FishType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime? ClosedSeasonFrom { get; set; }
    public DateTime? ClosedSeasonTo { get; set; }
    public int? MinimumSize { get; set; }
}