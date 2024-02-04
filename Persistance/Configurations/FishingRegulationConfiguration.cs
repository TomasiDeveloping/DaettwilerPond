using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    // Configuration class for the FishingRegulation entity to define its database schema.
    public class FishingRegulationConfiguration : IEntityTypeConfiguration<FishingRegulation>
    {
        // Configures the FishingRegulation entity.
        public void Configure(EntityTypeBuilder<FishingRegulation> builder)
        {
            // Primary key configuration
            builder.HasKey(fr => fr.Id);

            // Property configurations
            builder.Property(fr => fr.Regulation).IsRequired();
        }
    }
}