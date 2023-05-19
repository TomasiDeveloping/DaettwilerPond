using System.Reflection;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DaettwilerPondDbContext : IdentityDbContext<User, UserRole, Guid, IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DaettwilerPondDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Lsn50V2Lifecycle> Lsn50V2Lifecycles { get; set; }
    public DbSet<Lsn50V2Measurement> Lsn50V2Measurements { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorType> SensorTypes { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}