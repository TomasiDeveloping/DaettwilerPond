using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class LoginDto
{
    [Required(ErrorMessage = "Email ist ein Pflichtfeld")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Passwort ist ein Pflichtfeld")]
    public string Password { get; set; }
}