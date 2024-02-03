namespace Application.DataTransferObjects.CatchDetail
{
    // Data Transfer Object (DTO) representing the data for creating a fishing catch detail
    public class CreateCatchDetailDto
    {
        // Unique identifier of the associated fishing catch
        public Guid CatchId { get; set; }

        // Unique identifier of the fish type caught
        public Guid FishTypeId { get; set; }

        // Amount of the specific fish type caught
        public int Amount { get; set; }

        // Indicates whether crabs were present during the catch
        public bool HadCrabs { get; set; }
    }
}