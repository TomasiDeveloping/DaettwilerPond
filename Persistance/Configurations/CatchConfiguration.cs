using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CatchConfiguration : IEntityTypeConfiguration<Catch>
{
    public void Configure(EntityTypeBuilder<Catch> builder)
    {
        builder.HasKey(cs => cs.Id);
        builder.Property(cs => cs.CatchDate).IsRequired();
        builder.Property(cs => cs.HoursSpent).IsRequired();
        builder.HasOne(cs => cs.FishingLicense)
            .WithMany(l => l.Catches)
            .HasForeignKey(cs => cs.FishingLicenseId);
    }
}