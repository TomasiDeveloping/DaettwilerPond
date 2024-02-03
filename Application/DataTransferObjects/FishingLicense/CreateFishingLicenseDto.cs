namespace Application.DataTransferObjects.FishingLicense
{
    // Data Transfer Object (DTO) representing the data for creating a fishing license
    public class CreateFishingLicenseDto
    {
        // User identifier for whom the fishing license is created
        public Guid UserId { get; set; }

        // Year for which the fishing license is valid
        public int Year { get; set; }

        // Indicates whether the fishing license has been paid for
        public bool IsPaid { get; set; }

        // Indicates whether the fishing license is active
        public bool IsActive { get; set; }

        // Date when the fishing license expires
        public DateTime ExpiresOn { get; set; }
    }
}