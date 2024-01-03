namespace Application.DataTransferObjects.Catch;

public class CreateCatchDto
{
    public DateTime CatchDate { get; set; }
    public double? HoursSpent { get; set; } = 0;
    public Guid FishingLicenseId { get; set; }
    public DateTime? StartFishing { get; set; }
    public DateTime? EndFishing { get; set; }
}