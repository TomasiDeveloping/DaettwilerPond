namespace Application.DataTransferObjects.FishingLicense;

public class CreateFishingLicenseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int Year { get; set; }
    public bool IsPaid { get; set; }
    public bool IsActive { get; set; }
    public DateTime ExpiresOn { get; set; }
}