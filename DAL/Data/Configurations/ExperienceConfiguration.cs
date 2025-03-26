using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations;

/// <summary>
///     Configuration class for Experience entity.
/// </summary>
public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    /// <summary>
    ///     Configures the entity mappings and relationships for Experience.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        // Configure table name
        builder.ToTable("Experience");

        builder.HasKey(p => p.Id);

        // Configure required properties with constraints
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.LocationName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
        builder.Property(e => e.ExperienceType).IsRequired();

        // Configure many-to-one relationship with Developers
        builder.HasOne(e => e.Developer)
            .WithMany(d => d.Experiences)
            .HasForeignKey("DeveloperId")
            .IsRequired();
    }
}