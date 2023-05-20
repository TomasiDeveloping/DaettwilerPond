using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email ist ein Pflichtfeld")]
    [EmailAddress(ErrorMessage = "Keine gültige Emailadresse")]
    public string Email { get; set; }

    [Required(ErrorMessage = "ClientUri ist ein Pflichtfeld")]
    public string ClientUri { get; set; }
}