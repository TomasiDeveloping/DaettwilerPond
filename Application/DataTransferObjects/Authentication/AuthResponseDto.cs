namespace Application.DataTransferObjects.Authentication;

// Data Transfer Object (DTO) representing the authentication response
public class AuthResponseDto
{
    // Indicates whether the authentication was successful
    public bool IsSuccessful { get; set; }

    // Error message in case of authentication failure
    public string ErrorMessage { get; set; }

    // Authentication token generated upon successful authentication
    public string Token { get; set; }
}