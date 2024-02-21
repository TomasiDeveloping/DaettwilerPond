namespace Application.DataTransferObjects.Overseer;

public class OverseerMemberDetailsDto
{
    public string UserFullName { get; set; }
    public double TotalHours { get; set; }
    public int TotalFishes { get; set; }
    public bool IsLicenceActive { get; set; }
    public string LicenseIssuedBy { get; set; }
    public DateTime LicenseIssuedOn { get; set; }
    public DateTime LicenseValidUntil { get; set; }
    public bool IsLicencePaid { get; set; }
    public bool IsFishing { get; set; }
    public string SaNaNumber { get; set; }
}