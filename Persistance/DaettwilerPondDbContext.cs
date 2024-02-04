using System.Reflection;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.DataSeeding;

namespace Persistence;

// DaettwilerPondDbContext represents the database context for the Daettwiler Pond application.
public class DaettwilerPondDbContext(DbContextOptions options, IConfiguration configuration) : IdentityDbContext<User, UserRole, Guid, IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)
{

    // Define DbSet properties for each entity in the database.

    // Lifecycles of Lsn50V2 sensors
    public DbSet<Lsn50V2Lifecycle> Lsn50V2Lifecycles { get; set; }

    // Measurements recorded by Lsn50V2 sensors
    public DbSet<Lsn50V2Measurement> Lsn50V2Measurements { get; set; }

    // Sensors deployed in the application
    public DbSet<Sensor> Sensors { get; set; }

    // Types of sensors used in the application
    public DbSet<SensorType> SensorTypes { get; set; }

    // Addresses associated with users
    public DbSet<Address> Addresses { get; set; }

    // Types of fish in the application
    public DbSet<FishType> FishTypes { get; set; }

    // Regulations related to fishing
    public DbSet<FishingRegulation> FishingRegulations { get; set; }

    // Fishing licenses issued to users
    public DbSet<FishingLicense> FishingLicenses { get; set; }

    // Fishing clubs in the application
    public DbSet<FishingClub> FishingClubs { get; set; }

    // Details of individual catches
    public DbSet<CatchDetail> CatchDetails { get; set; }

    // Records of fishing catches
    public DbSet<Catch> Catches { get; set; }

    // Override the OnModelCreating method to configure entity relationships and apply data seeding.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Call the base class implementation for identity-related configurations.
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations from the current assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seed predefined roles and system administrator data.
        AddRoles.SeedRoles(modelBuilder);
        AddSystemAdministrator.SeedSystemAdministrator(modelBuilder, configuration.GetSection("SystemAdministrator"));
    }
}