namespace Application.DataTransferObjects.FishType;

public class FishTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime? ClosedSeasonFrom { get; set; }
    public DateTime? ClosedSeasonTo { get; set; }
    public int? MinimumSize { get; set; }
}