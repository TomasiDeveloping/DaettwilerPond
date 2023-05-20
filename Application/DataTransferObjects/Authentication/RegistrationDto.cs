using System.ComponentModel.DataAnnotations;

namespace Application.DataTransferObjects.Authentication;

public class RegistrationDto
{
    [Required(ErrorMessage = "Vorname ist ein Pflichtfeld")]
    [MaxLength(200, ErrorMessage = "Vorname darf maximum 200 Zeichen lang sein")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Nachname ist ein Pflichtfeld")]
    [MaxLength(200, ErrorMessage = "Nachname darf maximum 200 Zeichen lang sein")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email ist ein Pflichtfeld")]
    [EmailAddress(ErrorMessage = "Keine gültige Email Adresse")]
    public string Email { get; set; }

    public string Password { get; set; }
    [Required(ErrorMessage = "Role ist ein Pflichfeld")]
    public string Role { get; set; }

    [Required(ErrorMessage = "Aktiv ist ein Pflichfeld")]
    public bool IsActive { get; set; }
}