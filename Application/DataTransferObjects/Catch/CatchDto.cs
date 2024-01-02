using Application.DataTransferObjects.CatchDetail;
using Application.DataTransferObjects.FishingLicense;

namespace Application.DataTransferObjects.Catch;

public class CatchDto
{
    public Guid Id { get; set; }
    public DateTime CatchDate { get; set; }
    public double HoursSpent { get; set; }
    public Guid FishingLicenseId { get; set; }
    public FishingLicenseDto FishingLicense { get; set; }
    public ICollection<CatchDetailDto> CatchDetails { get; set; }
}