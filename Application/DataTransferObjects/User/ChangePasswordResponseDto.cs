namespace Application.DataTransferObjects.User
{
    // Data Transfer Object (DTO) representing the response for a password change operation
    public class ChangePasswordResponseDto
    {
        // Indicates whether the password change was successful
        public bool IsSuccessful { get; set; }

        // Error message (if any) in case of an unsuccessful password change
        public string ErrorMessage { get; set; }
    }
}