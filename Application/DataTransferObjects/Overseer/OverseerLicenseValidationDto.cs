namespace Application.DataTransferObjects.Overseer;

public class OverseerLicenseValidationDto
{
    public Guid LicenseId { get; set; }

    public string UserName { get; set; }

    public string UserSaNaNumber { get; set; }

    public DateTime UserBirthDate { get; set; }

    public DateTime ExpiryAt { get; set; }

    public string UserImageUrl { get; set; }

    public bool IsValid { get; set; }

    public bool IsActive { get; set; }

    public string ErrorMessage { get; set; }
}