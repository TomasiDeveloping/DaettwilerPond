namespace Application.DataTransferObjects.FishingRegulation
{
    // Data Transfer Object (DTO) representing information about a fishing regulation
    public class FishingRegulationDto
    {
        // Unique identifier for the fishing regulation
        public Guid Id { get; set; }

        // The content or description of the fishing regulation
        public string Regulation { get; set; }
    }
}