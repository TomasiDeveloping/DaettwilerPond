namespace Domain.Entities
{
    // Entity representing a fishing club within the application
    public class FishingClub
    {
        // Unique identifier for the fishing club
        public Guid Id { get; set; }

        // Name of the fishing club
        public string Name { get; set; }

        // Name associated with the billing address of the fishing club
        public string BillAddressName { get; set; }

        // Billing address of the fishing club
        public string BillAddress { get; set; }

        // Postal code of the billing address
        public string BillPostalCode { get; set; }

        // City of the billing address
        public string BillCity { get; set; }

        // IBAN number associated with the fishing club
        public string IbanNumber { get; set; }

        // License price set by the fishing club
        public decimal LicensePrice { get; set; }
    }
}