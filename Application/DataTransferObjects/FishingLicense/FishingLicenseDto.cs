namespace Application.DataTransferObjects.FishingLicense
{
    // Data Transfer Object (DTO) representing detailed information about a fishing license
    public class FishingLicenseDto
    {
        // Unique identifier for the fishing license
        public Guid Id { get; set; }

        // Full name of the user associated with the fishing license
        public string UserFullName { get; set; }

        // User identifier for whom the fishing license is created
        public Guid UserId { get; set; }

        // Date when the fishing license was created
        public DateTime CreatedAt { get; set; }

        // Date when the fishing license was last updated (nullable)
        public DateTime? UpdatedAt { get; set; }

        // Year for which the fishing license is valid
        public int Year { get; set; }

        // Indicates whether the fishing license has been paid for
        public bool IsPaid { get; set; }

        // Issuer of the fishing license
        public string IssuedBy { get; set; }

        // Indicates whether the fishing license is currently active
        public bool IsActive { get; set; }

        // Date when the fishing license expires
        public DateTime ExpiresOn { get; set; }
    }
}