// Importing necessary namespaces
using Application.DataTransferObjects.CatchDetail;

namespace Application.DataTransferObjects.Catch
{
    // Data Transfer Object (DTO) representing the data for a fishing catch
    public class CatchDto
    {
        // Unique identifier for the catch
        public Guid Id { get; set; }

        // Date when the catch occurred
        public DateTime CatchDate { get; set; }

        // Hours spent on the fishing activity
        public double HoursSpent { get; set; }

        // Start time of the fishing activity
        public DateTime? StartFishing { get; set; }

        // End time of the fishing activity
        public DateTime? EndFishing { get; set; }

        // Unique identifier of the fishing license associated with the catch
        public Guid FishingLicenseId { get; set; }

        // Amount of fish caught during the fishing activity
        public int AmountFishCatch { get; set; }

        // Collection of catch details providing additional information about the catch
        public ICollection<CatchDetailDto> CatchDetails { get; set; }
    }
}