namespace Application.DataTransferObjects.FishingLicense;

public class CreateFishingLicenseBillDto
{
    public Guid[] UserIds { get; set; }
    public int LicenseYear { get; set; }
    public string EmailMessage { get; set; }
}