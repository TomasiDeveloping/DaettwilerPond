using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class Lsn50V2LifecycleConfiguration : IEntityTypeConfiguration<Lsn50V2Lifecycle>
{
    public void Configure(EntityTypeBuilder<Lsn50V2Lifecycle> builder)
    {
        builder.HasKey(l => l.Id);
        builder.HasOne(l => l.Sensor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(l => l.ReceivedAt).IsRequired();
        builder.Property(l => l.BatteryLevel).IsRequired();
        builder.Property(l => l.BatteryVoltage).IsRequired().HasPrecision(2, 3);
    }
}