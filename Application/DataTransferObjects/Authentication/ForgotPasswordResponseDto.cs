namespace Application.DataTransferObjects.Authentication;

public class ForgotPasswordResponseDto
{
    public bool IsSuccessful { get; set; }
    public string ErrorMessage { get; set; }
}