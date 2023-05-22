using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FishingRegulationConfiguration : IEntityTypeConfiguration<FishingRegulation>
{
    public void Configure(EntityTypeBuilder<FishingRegulation> builder)
    {
        builder.HasKey(fr => fr.Id);
        builder.Property(fr => fr.Regulation).IsRequired();
    }
}