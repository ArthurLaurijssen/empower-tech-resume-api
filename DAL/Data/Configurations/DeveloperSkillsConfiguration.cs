using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations;

/// <summary>
///     Configuration class for DeveloperSkills entity.
/// </summary>
public class DeveloperSkillsConfiguration : IEntityTypeConfiguration<DeveloperSkill>
{
    /// <summary>
    ///     Configures the entity mappings and relationships for DeveloperSkills.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<DeveloperSkill> builder)
    {
        // Configure table name
        builder.ToTable("DeveloperSkills");

        // Configure primary key
        builder.HasKey(p => p.Id);

        // Configure required properties
        builder.Property(p => p.TechnologyName).IsRequired();
        builder.Property(p => p.ProficiencyLevel).IsRequired();

        // Configure many-to-one relationship with Developers
        builder.HasOne(p => p.Developer)
            .WithMany(d => d.DeveloperProficiencies)
            .HasForeignKey("DeveloperId")
            .IsRequired();

        // Configure the relationship with DevelopersSkillsUsedInProject
        builder.HasMany(p => p.Projects)
            .WithMany(p => p.DeveloperSkills)
            .UsingEntity<DeveloperSkillsUsedInProject>(
                j => j.HasOne(dp => dp.Project)
                    .WithMany()
                    .HasForeignKey(dp => dp.ProjectId),
                j => j.HasOne(dp => dp.DeveloperSkill)
                    .WithMany()
                    .HasForeignKey(dp => dp.DeveloperSkillsId)
            );
    }
}