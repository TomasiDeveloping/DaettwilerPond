using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.DataSeeding
{
    // Static class for seeding the system administrator into the database.
    public static class AddSystemAdministrator
    {
        // Method to seed the system administrator.
        public static void SeedSystemAdministrator(ModelBuilder builder, IConfigurationSection sysAdminSection)
        {
            // Extracting system administrator email from configuration.
            var systemAdministratorEmail = sysAdminSection["Email"];

            // Creating a password hasher for hashing the password.
            var passwordHasher = new PasswordHasher<User>();

            // Creating the system administrator user with predefined properties.
            var systemAdministrator = new User
            {
                Id = new Guid("1E59351A-D970-428D-B63E-A141578F0184"),
                FirstName = "System",
                LastName = "Administrator",
                Email = systemAdministratorEmail,
                UserName = systemAdministratorEmail,
                NormalizedUserName = systemAdministratorEmail!.ToUpper(),
                NormalizedEmail = systemAdministratorEmail!.ToLower(),
                EmailConfirmed = true,
                PasswordHash = passwordHasher.HashPassword(null!, sysAdminSection["password"]!),
                IsActive = true
            };

            // Seeding the system administrator user into the database.
            builder.Entity<User>().HasData(systemAdministrator);

            // Seeding the system administrator role into the database.
            builder.Entity<IdentityUserRole<Guid>>()
                .HasData(
                    new IdentityUserRole<Guid>
                    {
                        RoleId = new Guid("EA2A36E6-401F-4D44-AC17-E19E05FA3211"),
                        UserId = new Guid("1E59351A-D970-428D-B63E-A141578F0184")
                    });
        }
    }
}
