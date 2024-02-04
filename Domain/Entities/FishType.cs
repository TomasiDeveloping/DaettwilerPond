namespace Domain.Entities
{
    // Entity representing a type of fish within the application
    public class FishType
    {
        // Unique identifier for the fish type
        public Guid Id { get; set; }

        // The name of the fish type
        public string Name { get; set; }

        // The starting month of the closed season for fishing this type of fish
        public int? ClosedSeasonFromMonth { get; set; }

        // The starting day of the closed season for fishing this type of fish
        public int? ClosedSeasonFromDay { get; set; }

        // The ending month of the closed season for fishing this type of fish
        public int? ClosedSeasonToMonth { get; set; }

        // The ending day of the closed season for fishing this type of fish
        public int? ClosedSeasonToDay { get; set; }

        // Indicates whether the fish type has a closed season for fishing
        public bool HasClosedSeason { get; set; }

        // The minimum size requirement for catching this type of fish
        public int? MinimumSize { get; set; }

        // Indicates whether there is a minimum size requirement for catching this type of fish
        public bool HasMinimumSize { get; set; }

        // Collection of catch details associated with this fish type
        public ICollection<CatchDetail> CatchDetails { get; set; }
    }
}