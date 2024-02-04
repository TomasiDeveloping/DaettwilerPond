using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    // Configuration class for the User entity to define its database schema.
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        // Configures the User entity.
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Property configurations
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(250);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(250);
            builder.Property(u => u.IsActive).IsRequired();
        }
    }
}