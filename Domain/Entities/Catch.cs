namespace Domain.Entities
{
    // Entity representing a fishing catch within the application
    public class Catch
    {
        // Unique identifier for the fishing catch
        public Guid Id { get; set; }

        // Date and time when the fishing catch occurred
        public DateTime CatchDate { get; set; }

        // Hours spent during the fishing activity
        public double HoursSpent { get; set; }

        // Start time of the fishing activity
        public DateTime? StartFishing { get; set; }

        // End time of the fishing activity
        public DateTime? EndFishing { get; set; }

        // Unique identifier for the associated fishing license
        public Guid FishingLicenseId { get; set; }

        // Navigation property representing the associated fishing license
        public FishingLicense FishingLicense { get; set; }

        // Collection of catch details associated with this fishing catch
        public ICollection<CatchDetail> CatchDetails { get; set; }
    }
}