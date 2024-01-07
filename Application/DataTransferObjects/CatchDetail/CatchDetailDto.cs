namespace Application.DataTransferObjects.CatchDetail;

public class CatchDetailDto
{
    public Guid Id { get; set; }
    public Guid CatchId { get; set; }
    public Guid FishTypeId { get; set; }
    public string FishTypeName { get; set; }
    public int Amount { get; set; }
    public bool HadCrabs { get; set; }
}