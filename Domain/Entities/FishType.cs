namespace Domain.Entities;

public class FishType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int? ClosedSeasonFromMonth { get; set; }
    public int? ClosedSeasonFromDay { get; set; }
    public int? ClosedSeasonToMonth { get; set; }
    public int? ClosedSeasonToDay { get; set; }
    public bool HasClosedSeason { get; set; }
    public int? MinimumSize { get; set; }
    public bool HasMinimumSize { get; set; }
}