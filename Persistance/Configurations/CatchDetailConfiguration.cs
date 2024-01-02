using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CatchDetailConfiguration : IEntityTypeConfiguration<CatchDetail>
{
    public void Configure(EntityTypeBuilder<CatchDetail> builder)
    {
        builder.HasKey(cd => cd.Id);
        builder.Property(cd => cd.Amount).IsRequired();
        builder.Property(cd => cd.HadCrabs).IsRequired();
        builder.HasOne(cd => cd.Catch)
            .WithMany(cs => cs.CatchDetails)
            .HasForeignKey(cd => cd.CatchId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(cs => cs.FishType)
            .WithOne(ft => ft.CatchDetail)
            .HasForeignKey<CatchDetail>(cd => cd.FishTypeId);
    }
}