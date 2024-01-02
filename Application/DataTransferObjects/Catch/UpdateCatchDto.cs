namespace Application.DataTransferObjects.Catch;

public class UpdateCatchDto
{
    public Guid Id { get; set; }
    public DateTime CatchDate { get; set; }
    public double HoursSpent { get; set; }
}