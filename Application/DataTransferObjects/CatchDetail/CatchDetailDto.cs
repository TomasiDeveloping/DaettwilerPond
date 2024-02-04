namespace Application.DataTransferObjects.CatchDetail
{
    // Data Transfer Object (DTO) representing detailed information about a fishing catch
    public class CatchDetailDto
    {
        // Unique identifier for the catch detail
        public Guid Id { get; set; }

        // Unique identifier of the associated fishing catch
        public Guid CatchId { get; set; }

        // Unique identifier of the fish type caught
        public Guid FishTypeId { get; set; }

        // Name of the fish type caught
        public string FishTypeName { get; set; }

        // Amount of the specific fish type caught
        public int Amount { get; set; }

        // Indicates whether crabs were present during the catch
        public bool HadCrabs { get; set; }
    }
}