// Importing necessary namespace
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication
{
    // Data Transfer Object (DTO) representing the data for resetting the user's password
    public class ResetPasswordDto
    {
        // New password for the user
        [Required(ErrorMessage = "Password ist ein Pflichtfeld")]
        public string Password { get; set; }

        // Email address of the user requesting password reset
        public string Email { get; set; }

        // Token associated with the password reset request
        public string Token { get; set; }
    }
}