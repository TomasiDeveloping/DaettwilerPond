using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

// Data Transfer Object (DTO) representing the data for the "Forgot Password" functionality
public class ForgotPasswordDto
{
    // Email address for password recovery
    [Required(ErrorMessage = "Email ist ein Pflichtfeld")]
    [EmailAddress(ErrorMessage = "Keine gültige Emailadresse")]
    public string Email { get; set; }

    // Client URI for handling password reset on the client side
    [Required(ErrorMessage = "ClientUri ist ein Pflichtfeld")]
    public string ClientUri { get; set; }
}