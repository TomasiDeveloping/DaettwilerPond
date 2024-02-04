using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    // Configuration class for the Lsn50V2Measurement entity to define its database schema.
    public class Lsn50V2MeasurementConfiguration : IEntityTypeConfiguration<Lsn50V2Measurement>
    {
        // Configures the Lsn50V2Measurement entity.
        public void Configure(EntityTypeBuilder<Lsn50V2Measurement> builder)
        {
            // Primary key configuration
            builder.HasKey(l => l.Id);

            // Relationship configuration with the Sensor entity
            builder.HasOne(l => l.Sensor)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            // Property configurations
            builder.Property(l => l.DigitalStatus).IsRequired().HasMaxLength(50);
            builder.Property(l => l.ExtTrigger).IsRequired();
            builder.Property(l => l.Open).IsRequired();
            builder.Property(l => l.ReceivedAt).IsRequired();
            builder.Property(l => l.Temperature).IsRequired().HasPrecision(5, 2);
        }
    }
}