namespace Application.DataTransferObjects.Catch
{
    // Data Transfer Object (DTO) representing detailed yearly fishing catch information
    public class DetailYearlyCatch
    {
        // Month of the year
        public int Month { get; set; }

        // Number of fishes caught in the specified month
        public int Fishes { get; set; }

        // Total hours spent on fishing activities in the specified month
        public double HoursSpent { get; set; }
    }
}