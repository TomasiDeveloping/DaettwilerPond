using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FishTypeConfiguration : IEntityTypeConfiguration<FishType>

{
    public void Configure(EntityTypeBuilder<FishType> builder)
    {
        builder.HasKey(ft => ft.Id);
        builder.Property(ft => ft.Name).IsRequired().HasMaxLength(250);
        builder.Property(ft => ft.HasClosedSeason).IsRequired();
        builder.Property(ft => ft.HasMinimumSize).IsRequired();
    }
}