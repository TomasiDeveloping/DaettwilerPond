using Application.DataTransferObjects.Address;

namespace Application.DataTransferObjects.User
{
    // Data Transfer Object (DTO) representing a user with associated address details
    public class UserWithAddressDto
    {
        // Unique identifier for the user
        public Guid UserId { get; set; }

        // First name of the user
        public string FirstName { get; set; }

        // Last name of the user
        public string LastName { get; set; }

        // Email address of the user
        public string Email { get; set; }

        // Indicates whether the user account is active
        public bool IsActive { get; set; }

        // Role of the user
        public string Role { get; set; }

        public string SaNaNumber { get; set; }

        public string ImageUrl { get; set; }

        // Address details associated with the user
        public AddressDto Address { get; set; }
    }
}