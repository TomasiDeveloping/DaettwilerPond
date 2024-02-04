namespace Domain.Entities
{
    // Entity representing a fishing license within the application
    public class FishingLicense
    {
        // Unique identifier for the fishing license
        public Guid Id { get; set; }

        // User associated with the fishing license
        public User User { get; set; }

        // Unique identifier of the user associated with the fishing license
        public Guid UserId { get; set; }

        // Date and time when the fishing license was created
        public DateTime CreatedAt { get; set; }

        // Date and time when the fishing license was last updated (nullable)
        public DateTime? UpdatedAt { get; set; }

        // Collection of catches associated with the fishing license
        public ICollection<Catch> Catches { get; set; }

        // Year for which the fishing license is issued
        public int Year { get; set; }

        // Indicates whether the fishing license has been paid
        public bool IsPaid { get; set; }

        // Issuing authority or entity for the fishing license
        public string IssuedBy { get; set; }

        // Indicates whether the fishing license is currently active
        public bool IsActive { get; set; }

        // Date and time when the fishing license expires
        public DateTime ExpiresOn { get; set; }
    }
}