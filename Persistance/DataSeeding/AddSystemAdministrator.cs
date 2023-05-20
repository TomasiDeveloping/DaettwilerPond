using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.DataSeeding;

public class AddSystemAdministrator
{
    public static void SeedSystemAdministrator(ModelBuilder builder, IConfigurationSection sysAdminSection)
    {
        var systemAdministratorEmail = sysAdminSection["Email"];
        var passwordHasher = new PasswordHasher<User>();
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
        builder.Entity<User>().HasData(systemAdministrator);

        builder.Entity<IdentityUserRole<Guid>>()
            .HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = new Guid("EA2A36E6-401F-4D44-AC17-E19E05FA3211"),
                    UserId = new Guid("1E59351A-D970-428D-B63E-A141578F0184")
                });
    }
}