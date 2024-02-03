namespace Application.DataTransferObjects.User
{
    // Data Transfer Object (DTO) representing the data for changing a user's password
    public class ChangePasswordDto
    {
        // Unique identifier for the user
        public Guid UserId { get; set; }

        // Current password of the user
        public string CurrentPassword { get; set; }

        // New password for the user
        public string Password { get; set; }

        // Confirmation of the new password to ensure accuracy
        public string ConfirmPassword { get; set; }
    }
}