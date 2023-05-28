using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FishingLicenseConfiguration : IEntityTypeConfiguration<FishingLicense>
{
    public void Configure(EntityTypeBuilder<FishingLicense> builder)
    {
        builder.HasKey(l => l.Id);
        builder.HasOne(l => l.User)
            .WithMany(u => u.FishingLicenses)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(l => l.CreatedAt).IsRequired();
        builder.Property(l => l.ExpiresOn).IsRequired();
        builder.Property(l => l.IsPaid).IsRequired().HasMaxLength(200);
        builder.Property(l => l.IssuedBy).IsRequired();
        builder.Property(l => l.UpdatedAt).IsRequired(false);
        builder.Property(l => l.Year).IsRequired();
        builder.Property(l => l.IsActive).IsRequired();
    }
}