using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    // Configuration class for the FishType entity to define its database schema.
    public class FishTypeConfiguration : IEntityTypeConfiguration<FishType>
    {
        // Configures the FishType entity.
        public void Configure(EntityTypeBuilder<FishType> builder)
        {
            // Primary key configuration
            builder.HasKey(ft => ft.Id);

            // Property configurations
            builder.Property(ft => ft.Name).IsRequired().HasMaxLength(250);
            builder.Property(ft => ft.HasClosedSeason).IsRequired();
            builder.Property(ft => ft.HasMinimumSize).IsRequired();
        }
    }
}