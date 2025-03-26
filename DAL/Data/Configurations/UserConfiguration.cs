using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations;

/// <summary>
///     Configuration class for Users entity.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    ///     Configures the entity mappings and relationships for Users.
    /// </summary>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configure table name
        builder.ToTable("Users");

        // Configure primary key
        builder.HasKey(u => u.Id);

        builder.Property(u => u.ExternalId)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(u => u.ExternalId)
            .IsUnique();


        // Configure one-to-many relationship with Permission
        builder.HasMany(u => u.Permissions)
            .WithOne()
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}