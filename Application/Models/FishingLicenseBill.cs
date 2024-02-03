namespace Application.Models;

// Model representing a fishing license bill
public class FishingLicenseBill
{
    // Account number for the bill
    public string Account { get; set; }

    // Name of the creditor for the bill
    public string CreditorName { get; set; }

    // Address of the creditor for the bill
    public string CreditorAddress { get; set; }

    // City of the creditor for the bill
    public string CreditorCity { get; set; }

    // Amount of the bill
    public decimal Amount { get; set; }

    // Name of the debtor for the bill
    public string DebtorName { get; set; }

    // Address of the debtor for the bill
    public string DebtorAddress { get; set; }

    // City of the debtor for the bill
    public string DebtorCity { get; set; }

    // Reference message for the bill
    public string ReferenceMessage { get; set; }

    // Name of the fishing club associated with the bill
    public string FishingClubName { get; set; }

    // Year for which the fishing license is issued
    public string LicenseYear { get; set; }

    // Date when the invoice is generated (defaults to current date and time)
    public DateTime InvoiceDate { get; set; } = DateTime.Now;
}