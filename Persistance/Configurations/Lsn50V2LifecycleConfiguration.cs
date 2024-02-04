using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    // Configuration class for the Lsn50V2Lifecycle entity to define its database schema.
    public class Lsn50V2LifecycleConfiguration : IEntityTypeConfiguration<Lsn50V2Lifecycle>
    {
        // Configures the Lsn50V2Lifecycle entity.
        public void Configure(EntityTypeBuilder<Lsn50V2Lifecycle> builder)
        {
            // Primary key configuration
            builder.HasKey(l => l.Id);

            // Relationship configuration with the Sensor entity
            builder.HasOne(l => l.Sensor)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            // Property configurations
            builder.Property(l => l.ReceivedAt).IsRequired();
            builder.Property(l => l.BatteryLevel).IsRequired();
            builder.Property(l => l.BatteryVoltage).IsRequired().HasPrecision(5, 3);
        }
    }
}