namespace Application.DataTransferObjects.Authentication;

public class ResetPasswordResponseDto
{
    public bool IsSuccessful { get; set; }
    public IEnumerable<string> Errors { get; set; }
}