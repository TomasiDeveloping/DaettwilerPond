using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class SensorTypeConfiguration : IEntityTypeConfiguration<SensorType>
{
    public void Configure(EntityTypeBuilder<SensorType> builder)
    {
        builder.HasKey(st => st.Id);
        builder.Property(st => st.Name).IsRequired().HasMaxLength(150);
        builder.Property(st => st.Producer).IsRequired().HasMaxLength(250);
    }
}