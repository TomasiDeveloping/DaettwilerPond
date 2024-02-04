using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    // Configuration class for the SensorType entity to define its database schema.
    public class SensorTypeConfiguration : IEntityTypeConfiguration<SensorType>
    {
        // Configures the SensorType entity.
        public void Configure(EntityTypeBuilder<SensorType> builder)
        {
            // Primary key configuration
            builder.HasKey(st => st.Id);

            // Property configurations
            builder.Property(st => st.Name).IsRequired().HasMaxLength(150);
            builder.Property(st => st.Producer).IsRequired().HasMaxLength(250);
        }
    }
}