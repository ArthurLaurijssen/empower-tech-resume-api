using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations;

/// <summary>
///     Configuration class for Permission entity.
/// </summary>
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <summary>
    ///     Configures the entity mappings and relationships for Permission.
    /// </summary>
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Configure table name
        builder.ToTable("Permission");

        // Configure primary key
        builder.HasKey(p => p.Id);

        // Configure required properties with constraints
        builder.Property(p => p.Resource)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ResourceId)
            .HasMaxLength(100);

        builder.Property(p => p.Scope)
            .IsRequired()
            .HasConversion<string>();

        // Create index for faster permission lookups
        builder.HasIndex(p => new { p.Resource, p.ResourceId, p.Scope });
    }
}