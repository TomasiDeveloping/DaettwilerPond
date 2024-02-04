namespace Application.DataTransferObjects.Authentication
{
    // Data Transfer Object (DTO) representing the response for user registration
    public class RegistrationResponseDto
    {
        // Indicates whether the user registration was successful
        public bool IsSuccessful { get; set; }

        // Collection of error messages in case of registration failure
        public IEnumerable<string> Errors { get; set; }
    }
}