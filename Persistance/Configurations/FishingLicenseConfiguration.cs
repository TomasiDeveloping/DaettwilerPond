using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    // Configuration class for the FishingLicense entity to define its database schema.
    public class FishingLicenseConfiguration : IEntityTypeConfiguration<FishingLicense>
    {
        // Configures the FishingLicense entity.
        public void Configure(EntityTypeBuilder<FishingLicense> builder)
        {
            // Primary key configuration
            builder.HasKey(l => l.Id);

            // Relationship configuration with User entity
            builder.HasOne(l => l.User)
                .WithMany(u => u.FishingLicenses)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Property configurations
            builder.Property(l => l.CreatedAt).IsRequired();
            builder.Property(l => l.ExpiresOn).IsRequired();
            builder.Property(l => l.IsPaid).IsRequired(); // Note: Removed unnecessary MaxLength
            builder.Property(l => l.IssuedBy).HasMaxLength(100).IsRequired();
            builder.Property(l => l.UpdatedAt).IsRequired(false);
            builder.Property(l => l.Year).IsRequired();
            builder.Property(l => l.IsActive).IsRequired();
        }
    }
}