using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    // Configuration class for the Sensor entity to define its database schema.
    public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
    {
        // Configures the Sensor entity.
        public void Configure(EntityTypeBuilder<Sensor> builder)
        {
            // Primary key configuration
            builder.HasKey(s => s.Id);

            // Relationship configuration with the SensorType entity
            builder.HasOne(s => s.SensorType)
                .WithMany(st => st.Sensors)
                .HasForeignKey(s => s.SensorTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Property configurations
            builder.Property(s => s.CreatedAt).IsRequired();
            builder.Property(s => s.DevEui).IsRequired().HasMaxLength(16);
        }
    }
}