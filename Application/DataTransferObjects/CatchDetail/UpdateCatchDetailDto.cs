namespace Application.DataTransferObjects.CatchDetail;

public class UpdateCatchDetailDto
{
    public Guid Id { get; set; }
    public Guid CatchId { get; set; }
    public Guid FishTypeId { get; set; }
    public int Amount { get; set; }
    public bool HadCrabs { get; set; }
}