namespace Application.DataTransferObjects.Authentication;

public class RegistrationResponseDto
{
    public bool IsSuccessful { get; set; }
    public IEnumerable<string> Errors { get; set; }
}