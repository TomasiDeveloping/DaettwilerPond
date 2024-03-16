namespace Application.DataTransferObjects.FishingLicense;

public class ValidateResponse
{
    public Guid LicenseId { get; set; }

    public string UserName { get; set; }

    public bool IsValid { get; set; }

    public string ErrorMessage { get; set; }
}