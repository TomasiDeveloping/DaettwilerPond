namespace Application.DataTransferObjects.Authentication;

// Data Transfer Object (DTO) representing the response for the "Forgot Password" functionality
public class ForgotPasswordResponseDto
{
    // Indicates whether the "Forgot Password" request was successful
    public bool IsSuccessful { get; set; }

    // Error message in case of failure during "Forgot Password" request
    public string ErrorMessage { get; set; }
}