namespace Application.DataTransferObjects.FishingClub
{
    // Data Transfer Object (DTO) representing detailed information about a fishing club
    public class FishingClubDto
    {
        // Unique identifier for the fishing club
        public Guid Id { get; set; }

        // Name of the fishing club
        public string Name { get; set; }

        // Billing address name of the fishing club
        public string BillAddressName { get; set; }

        // Billing address of the fishing club
        public string BillAddress { get; set; }

        // Billing postal code of the fishing club
        public string BillPostalCode { get; set; }

        // Billing city of the fishing club
        public string BillCity { get; set; }

        // IBAN (International Bank Account Number) of the fishing club
        public string IbanNumber { get; set; }

        // License price associated with the fishing club
        public decimal LicensePrice { get; set; }
    }
}