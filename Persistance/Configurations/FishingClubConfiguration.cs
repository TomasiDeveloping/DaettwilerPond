using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FishingClubConfiguration : IEntityTypeConfiguration<FishingClub>
{
    public void Configure(EntityTypeBuilder<FishingClub> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(fc => fc.BillAddress).IsRequired().HasMaxLength(70);
        builder.Property(fc => fc.BillAddressName).IsRequired().HasMaxLength(150);
        builder.Property(fc => fc.BillCity).IsRequired().HasMaxLength(70);
        builder.Property(fc => fc.BillPostalCode).IsRequired().HasMaxLength(5);
        builder.Property(fc => fc.IbanNumber).IsRequired().HasMaxLength(50);
        builder.Property(fc => fc.LicensePrice).IsRequired().HasPrecision(8, 2);
        builder.Property(fc => fc.Name).IsRequired().HasMaxLength(250);
    }
}