namespace Application.DataTransferObjects.FishType
{
    // Data Transfer Object (DTO) representing the data for creating a fish type
    public class CreateFishTypeDto
    {
        // Name of the fish type
        public string Name { get; set; }

        // Month of the starting month of the closed season (nullable)
        public int? ClosedSeasonFromMonth { get; set; }

        // Day of the starting day of the closed season (nullable)
        public int? ClosedSeasonFromDay { get; set; }

        // Month of the ending month of the closed season (nullable)
        public int? ClosedSeasonToMonth { get; set; }

        // Day of the ending day of the closed season (nullable)
        public int? ClosedSeasonToDay { get; set; }

        // Indicates whether the fish type has a closed season
        public bool HasClosedSeason { get; set; }

        // Minimum size requirement for the fish type (nullable)
        public int? MinimumSize { get; set; }

        // Indicates whether the fish type has a minimum size requirement
        public bool HasMinimumSize { get; set; }
    }
}