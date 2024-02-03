namespace Application.DataTransferObjects.User
{
    // Data Transfer Object (DTO) representing basic information about a user
    public class UserDto
    {
        // Unique identifier for the user
        public Guid Id { get; set; }

        // First name of the user
        public string FirstName { get; set; }

        // Last name of the user
        public string LastName { get; set; }

        // Email address of the user
        public string Email { get; set; }

        // Indicates whether the user account is active
        public bool IsActive { get; set; }
    }
}