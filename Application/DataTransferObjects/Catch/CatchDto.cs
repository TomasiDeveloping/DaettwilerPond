using Application.DataTransferObjects.CatchDetail;

namespace Application.DataTransferObjects.Catch;

public class CatchDto
{
    public Guid Id { get; set; }
    public DateTime CatchDate { get; set; }
    public double HoursSpent { get; set; }
    public DateTime? StartFishing { get; set; }
    public DateTime? EndFishing { get; set; }
    public Guid FishingLicenseId { get; set; }
    public int AmountFishCatch { get; set; }
    public ICollection<CatchDetailDto> CatchDetails{ get; set; }
}