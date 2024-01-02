namespace Domain.Entities;

public class Catch
{
    public Guid Id { get; set; }
    public DateTime CatchDate { get; set; }
    public double HoursSpent { get; set; }
    public Guid FishingLicenseId { get; set; }
    public FishingLicense FishingLicense { get; set; }
    public ICollection<CatchDetail> CatchDetails { get; set; }
}