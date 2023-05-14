using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.SensorType)
            .WithMany(st => st.Sensors)
            .HasForeignKey(s => s.SensorTypeId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.DevEui).IsRequired().HasMaxLength(16);
    }
}