using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DataSeeding
{
    // Static class for seeding user roles into the database.
    public static class AddRoles
    {
        // Method to seed user roles.
        public static void SeedRoles(ModelBuilder builder)
        {
            // List of predefined user roles with their properties.
            var roles = new List<UserRole>
            {
                new()
                {
                    Name = RoleConstants.Administrator,
                    NormalizedName = RoleConstants.Administrator.ToUpper(),
                    Id = new Guid("EA2A36E6-401F-4D44-AC17-E19E05FA3211")
                },
                new()
                {
                    Name = RoleConstants.Overseer,
                    NormalizedName = RoleConstants.Overseer.ToUpper(),
                    Id = new Guid("F63C078A-DC82-4C07-A01B-0F0F0730882B")
                },
                new()
                {
                    Name = RoleConstants.User,
                    NormalizedName = RoleConstants.User.ToUpper(),
                    Id = new Guid("3939759C-F6A1-4A5B-A28F-F99E8DCCA83E")
                }
            };

            // Configuring and seeding user roles into the database.
            builder.Entity<UserRole>()
                .HasData(roles);
        }
    }
}