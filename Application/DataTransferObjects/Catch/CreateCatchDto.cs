namespace Application.DataTransferObjects.Catch
{
    // Data Transfer Object (DTO) representing the data for creating a fishing catch
    public class CreateCatchDto
    {
        // Date when the catch is created
        public DateTime CatchDate { get; set; }

        // Hours spent on the fishing activity (default to 0 if not provided)
        public double? HoursSpent { get; set; } = 0;

        // Unique identifier of the fishing license associated with the catch
        public Guid FishingLicenseId { get; set; }

        // Start time of the fishing activity
        public DateTime? StartFishing { get; set; }

        // End time of the fishing activity
        public DateTime? EndFishing { get; set; }
    }
}