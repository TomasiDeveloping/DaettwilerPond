namespace Domain.Entities;

public class FishingClub
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string BillAddressName { get; set; }
    public string BillAddress { get; set; }
    public string BillPostalCode { get; set; }
    public string BillCity { get; set; }
    public string IbanNumber { get; set; }
    public decimal LicensePrice { get; set; }
}