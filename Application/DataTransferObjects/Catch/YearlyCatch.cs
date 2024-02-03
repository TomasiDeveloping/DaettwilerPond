namespace Application.DataTransferObjects.Catch
{
    // Data Transfer Object (DTO) representing yearly fishing catch summary information
    public class YearlyCatch
    {
        // Total number of fishes caught in a year
        public int FishCatches { get; set; }

        // Total hours spent on fishing activities in a year
        public double HoursSpent { get; set; }
    }
}