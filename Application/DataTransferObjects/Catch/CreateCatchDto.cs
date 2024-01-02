namespace Application.DataTransferObjects.Catch;

public class CreateCatchDto
{
    public DateTime CatchDate { get; set; }
    public double HoursSpent { get; set; }
    public Guid FishingLicenseId { get; set; }
}