// Importing necessary namespaces
using System.ComponentModel.DataAnnotations;
using Application.DataTransferObjects.Address;

namespace Application.DataTransferObjects.Authentication
{
    // Data Transfer Object (DTO) representing the data for user registration
    public class RegistrationDto
    {
        // First name of the user (limited to 200 characters)
        [Required(ErrorMessage = "Vorname ist ein Pflichtfeld")]
        [MaxLength(200, ErrorMessage = "Vorname darf maximum 200 Zeichen lang sein")]
        public string FirstName { get; set; }

        // Last name of the user (limited to 200 characters)
        [Required(ErrorMessage = "Nachname ist ein Pflichtfeld")]
        [MaxLength(200, ErrorMessage = "Nachname darf maximum 200 Zeichen lang sein")]
        public string LastName { get; set; }

        // Email address for user registration
        [Required(ErrorMessage = "Email ist ein Pflichtfeld")]
        [EmailAddress(ErrorMessage = "Keine gültige Email Adresse")]
        public string Email { get; set; }

        // Role assigned to the user
        [Required(ErrorMessage = "Role ist ein Pflichfeld")]
        public string Role { get; set; }

        // Indicates whether the user is active
        [Required(ErrorMessage = "Aktiv ist ein Pflichfeld")]
        public bool IsActive { get; set; }

        // Address information associated with the user
        public AddressDto Address { get; set; }
    }
}