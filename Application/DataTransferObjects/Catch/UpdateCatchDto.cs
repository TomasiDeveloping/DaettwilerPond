namespace Application.DataTransferObjects.Catch
{
    // Data Transfer Object (DTO) representing the data for updating a fishing catch
    public class UpdateCatchDto
    {
        // Unique identifier of the catch to be updated
        public Guid Id { get; set; }

        // Date when the catch is updated
        public DateTime CatchDate { get; set; }

        // Hours spent on the fishing activity (default to 0 if not provided)
        public double? HoursSpent { get; set; } = 0;

        // Start time of the fishing activity
        public DateTime? StartFishing { get; set; }

        // End time of the fishing activity
        public DateTime? EndFishing { get; set; }
    }
}