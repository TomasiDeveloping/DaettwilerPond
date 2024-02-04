// Importing necessary namespace
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication
{
    // Data Transfer Object (DTO) representing the data for user login
    public class LoginDto
    {
        // Email address for user login
        [Required(ErrorMessage = "Email ist ein Pflichtfeld")]
        public string Email { get; set; }

        // Password for user login
        [Required(ErrorMessage = "Passwort ist ein Pflichtfeld")]
        public string Password { get; set; }
    }
}