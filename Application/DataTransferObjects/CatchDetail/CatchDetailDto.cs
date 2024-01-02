using Application.DataTransferObjects.Catch;
using Application.DataTransferObjects.FishType;

namespace Application.DataTransferObjects.CatchDetail;

public class CatchDetailDto
{
    public Guid Id { get; set; }
    public Guid CatchId { get; set; }
    public CatchDto Catch { get; set; }
    public Guid FishTypeId { get; set; }
    public FishTypeDto FishType { get; set; }
    public int Amount { get; set; }
    public bool HadCrabs { get; set; }
}