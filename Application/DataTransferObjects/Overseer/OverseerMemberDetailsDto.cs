namespace Application.DataTransferObjects.Overseer;

public class OverseerMemberDetailsDto
{
    public double TotalHours { get; set; }
    public int TotalFishes { get; set; }
    public bool IsLicenceActive { get; set; }
    public string LicenseIssuedBy { get; set; }
    public DateTime LicenseIssuedOn { get; set; }
    public DateTime LicenseValidUntil { get; set; }
    public bool IsLicencePaid { get; set; }
    public bool IsFishing { get; set; }
}