using DAL.Data.Configurations;
using Domain.Entities;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

/// <summary>
///     Represents the database context for the application, handling all entity configurations and relationships.
/// </summary>
public class ResumeDbContext : DbContext
{
    /// <summary>
    ///     Initializes a new instance of the ResumeDbContext.
    /// </summary>
    /// <param name="options">The options to be used by DbContext.</param>
    public ResumeDbContext(DbContextOptions<ResumeDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Gets or sets the Developers DbSet.
    /// </summary>
    public DbSet<Developer> Developers { get; set; }

    /// <summary>
    ///     Gets or sets the DeveloperSkills DbSet.
    /// </summary>
    public DbSet<DeveloperSkill> DeveloperSkills { get; set; }

    /// <summary>
    ///     Gets or sets the Users DbSet.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    ///     Gets or sets the Permission DbSet.
    /// </summary>
    public DbSet<Permission> Permissions { get; set; }

    /// <summary>
    ///     Gets or sets the Experiences DbSet.
    /// </summary>
    public DbSet<Experience> Experiences { get; set; }


    /// <summary>
    ///     Gets or sets the Projects DbSet.
    /// </summary>
    public DbSet<Project> Projects { get; set; }

    /// <summary>
    ///     Gets or sets the SocialMediaLinks DbSet.
    /// </summary>
    public DbSet<SocialMediaLink> SocialMediaLinks { get; set; }


    /// <summary>
    ///     Configures the model that was discovered by convention from the entity types.
    /// </summary>
    /// <param name="builder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Apply all entity configurations
        builder.ApplyConfiguration(new DeveloperConfiguration());
        builder.ApplyConfiguration(new DeveloperSkillsConfiguration());
        builder.ApplyConfiguration(new ExperienceConfiguration());
        builder.ApplyConfiguration(new ProjectConfiguration());
        builder.ApplyConfiguration(new SocialMediaLinkConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new PermissionConfiguration());
    }
}