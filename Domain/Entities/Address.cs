namespace Domain.Entities
{
    // Entity representing a user's address within the application
    public class Address
    {
        // Unique identifier for the address
        public Guid Id { get; set; }

        // User associated with this address
        public User User { get; set; }

        // Unique identifier for the associated user
        public Guid UserId { get; set; }

        // Street name of the address
        public string Street { get; set; }

        // House number of the address
        public string HouseNumber { get; set; }

        // City where the address is located
        public string City { get; set; }

        // Country where the address is located
        public string Country { get; set; }

        // Postal code of the address
        public string PostalCode { get; set; }

        // Phone number associated with the address
        public string Phone { get; set; }

        // Mobile number associated with the address
        public string Mobile { get; set; }
    }
}