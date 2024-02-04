using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    // Configuration class for the CatchDetail entity to define its database schema.
    public class CatchDetailConfiguration : IEntityTypeConfiguration<CatchDetail>
    {
        // Configures the CatchDetail entity.
        public void Configure(EntityTypeBuilder<CatchDetail> builder)
        {
            // Primary key configuration
            builder.HasKey(cd => cd.Id);

            // Property configurations
            builder.Property(cd => cd.Amount).IsRequired();
            builder.Property(cd => cd.HadCrabs).IsRequired();

            // Relationship configuration with Catch entity
            builder.HasOne(cd => cd.Catch)
                .WithMany(cs => cs.CatchDetails)
                .HasForeignKey(cd => cd.CatchId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relationship configuration with FishType entity
            builder.HasOne(cd => cd.FishType)
                .WithMany(ft => ft.CatchDetails)
                .HasForeignKey(cd => cd.FishTypeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}