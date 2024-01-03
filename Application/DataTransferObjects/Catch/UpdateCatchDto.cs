namespace Application.DataTransferObjects.Catch;

public class UpdateCatchDto
{
    public Guid Id { get; set; }
    public DateTime CatchDate { get; set; }
    public double? HoursSpent { get; set; } = 0;
    public DateTime? StartFishing { get; set; }
    public DateTime? EndFishing { get; set; }
}