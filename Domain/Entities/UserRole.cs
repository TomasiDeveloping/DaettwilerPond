using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    // Represents a role in the application, extending IdentityRole with a Guid as the key
    public class UserRole : IdentityRole<Guid>
    {
        // No additional properties for now, inherits base properties from IdentityRole
    }
}