using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

// Configuration class for the Catch entity to define its database schema.
public class CatchConfiguration : IEntityTypeConfiguration<Catch>
{
    // Configures the Catch entity.
    public void Configure(EntityTypeBuilder<Catch> builder)
    {
        // Primary key configuration
        builder.HasKey(cs => cs.Id);

        // Property configurations
        builder.Property(cs => cs.CatchDate).IsRequired();
        builder.Property(cs => cs.HoursSpent).IsRequired();
        builder.Property(c => c.StartFishing).IsRequired(false);
        builder.Property(c => c.EndFishing).IsRequired(false);

        // Relationship configuration with FishingLicense entity
        builder.HasOne(cs => cs.FishingLicense)
            .WithMany(l => l.Catches)
            .HasForeignKey(cs => cs.FishingLicenseId);
    }
}