namespace Application.DataTransferObjects.Authentication
{
    // Data Transfer Object (DTO) representing the response for resetting the user's password
    public class ResetPasswordResponseDto
    {
        // Indicates whether the password reset was successful
        public bool IsSuccessful { get; set; }

        // Collection of error messages in case of password reset failure
        public IEnumerable<string> Errors { get; set; }
    }
}