using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class Lsn50V2MeasurementConfiguration : IEntityTypeConfiguration<Lsn50V2Measurement>
{
    public void Configure(EntityTypeBuilder<Lsn50V2Measurement> builder)
    {
        builder.HasKey(l => l.Id);
        builder.HasOne(l => l.Sensor)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(l => l.DigitalStatus).IsRequired().HasMaxLength(50);
        builder.Property(l => l.ExtTrigger).IsRequired();
        builder.Property(l => l.Open).IsRequired();
        builder.Property(l => l.ReceivedAt).IsRequired();
        builder.Property(l => l.Temperature).IsRequired().HasPrecision(3, 2);
    }
}