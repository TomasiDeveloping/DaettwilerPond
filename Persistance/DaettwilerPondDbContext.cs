using System.Reflection;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public class DaettwilerPondDbContext : IdentityDbContext<User, UserRole, Guid, IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    private readonly IConfiguration _configuration;

    public DaettwilerPondDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Lsn50V2Lifecycle> Lsn50V2Lifecycles { get; set; }
    public DbSet<Lsn50V2Measurement> Lsn50V2Measurements { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorType> SensorTypes { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<FishType> FishTypes { get; set; }
    public DbSet<FishingRegulation> FishingRegulations { get; set; }
    public DbSet<FishingLicense> FishingLicenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Seeding Data
        DataSeeding.AddRoles.SeedRoles(modelBuilder);
        DataSeeding.AddSystemAdministrator.SeedSystemAdministrator(modelBuilder, _configuration.GetSection("SystemAdministrator"));
    }
}