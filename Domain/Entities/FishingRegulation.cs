namespace Domain.Entities
{
    // Entity representing a fishing regulation within the application
    public class FishingRegulation
    {
        // Unique identifier for the fishing regulation
        public Guid Id { get; set; }

        // Textual representation of the fishing regulation
        public string Regulation { get; set; }
    }
}