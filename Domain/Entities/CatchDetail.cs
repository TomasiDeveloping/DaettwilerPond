namespace Domain.Entities
{
    // Entity representing details of a fish catch within a larger fishing catch
    public class CatchDetail
    {
        // Unique identifier for the catch detail
        public Guid Id { get; set; }

        // Unique identifier for the associated fishing catch
        public Guid CatchId { get; set; }

        // Navigation property representing the associated fishing catch
        public Catch Catch { get; set; }

        // Unique identifier for the associated fish type
        public Guid FishTypeId { get; set; }

        // Navigation property representing the associated fish type
        public FishType FishType { get; set; }

        // Amount of fish caught in this specific detail
        public int Amount { get; set; }

        // Flag indicating whether crabs were present during the catch
        public bool HadCrabs { get; set; }
    }
}