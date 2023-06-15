namespace Application.Models;

public class FishingLicenseBill
{
    public string Account { get; set; }
    public string CreditorName { get; set; }
    public string CreditorAddress { get; set; }
    public string CreditorCity { get; set; }
    public decimal Amount { get; set; }
    public string DebtorName { get; set; }
    public string DebtorAddress { get; set; }
    public string DebtorCity { get; set; }
    public string ReferenceMessage { get; set; }
    public string FishingClubName { get; set; }
    public string LicenseYear { get; set; }
    public DateTime InvoiceDate { get; set; } = DateTime.Now;
}