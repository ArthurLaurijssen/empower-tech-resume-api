using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations;

/// <summary>
///     Configuration class for Developers entity.
/// </summary>
public class DeveloperConfiguration : IEntityTypeConfiguration<Developer>
{
    /// <summary>
    ///     Configures the entity mappings and relationships for Developers.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Developer> builder)
    {
        // Configure table name
        builder.ToTable("Developers");

        // Configure primary key
        builder.HasKey(p => p.Id);

        // Configure required properties with constraints
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Email).IsRequired().HasMaxLength(254);
        builder.Property(d => d.ItExperienceStartDate).IsRequired();
        builder.Property(d => d.WorkExperienceStartDate).IsRequired();

        // Configure owned entity Greeting
        builder.OwnsOne(d => d.Greeting, g =>
        {
            g.Property(p => p.Title).IsRequired().HasMaxLength(100);
            g.Property(p => p.Message).IsRequired().HasMaxLength(500);
        });

        // Configure owned entity Mission
        builder.OwnsOne(d => d.Mission, m =>
        {
            m.Property(p => p.Title).IsRequired().HasMaxLength(100);
            m.Property(p => p.Description).IsRequired().HasMaxLength(1000);
        });

        // Configure one-to-many relationships with cascade delete
        builder.HasMany(d => d.Experiences)
            .WithOne()
            .HasForeignKey("DeveloperId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.SocialMediaLinks)
            .WithOne()
            .HasForeignKey("DeveloperId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.DeveloperProficiencies)
            .WithOne()
            .HasForeignKey("DeveloperId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}