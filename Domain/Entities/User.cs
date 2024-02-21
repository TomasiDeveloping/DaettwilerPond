using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    // Represents a user in the application, extending IdentityUser with a Guid as the key
    public class User : IdentityUser<Guid>
    {
        // First name of the user
        public string FirstName { get; set; }

        // Last name of the user
        public string LastName { get; set; }

        // Indicates whether the user is currently active
        public bool IsActive { get; set; }

        // The timestamp of the user's last activity (nullable)
        public DateTime? LastActivity { get; set; }

        // SaNa number
        public string SaNaNumber { get; set; }

        // Collection of addresses associated with the user
        public ICollection<Address> Addresses { get; set; }

        // Collection of fishing licenses associated with the user
        public ICollection<FishingLicense> FishingLicenses { get; set; }
    }
}