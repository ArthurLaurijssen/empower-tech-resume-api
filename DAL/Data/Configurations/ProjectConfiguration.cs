using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations;

/// <summary>
///     Configuration class for Project entity.
/// </summary>
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    /// <summary>
    ///     Configures the entity mappings and relationships for Project.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        // Configure table name
        builder.ToTable("Projects");

        // Configure primary key
        builder.HasKey(p => p.Id);

        // Configure required properties
        builder.Property(p => p.ImageUrl).IsRequired();
        builder.Property(p => p.Title).IsRequired();
        builder.Property(p => p.Description).IsRequired();

        // Configure the relationship with DevelopersSkillsUsedInProject
        builder.HasMany(p => p.DeveloperSkills)
            .WithMany(d => d.Projects)
            .UsingEntity<DeveloperSkillsUsedInProject>();
    }
}