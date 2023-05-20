using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Password ist ein Pflichtfeld")]
    public string Password { get; set; }

    public string Email { get; set; }
    public string Token { get; set; }
}