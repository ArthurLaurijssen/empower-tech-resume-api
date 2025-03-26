using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations;

/// <summary>
///     Configuration class for SocialMediaLink entity.
/// </summary>
public class SocialMediaLinkConfiguration : IEntityTypeConfiguration<SocialMediaLink>
{
    /// <summary>
    ///     Configures the entity mappings and relationships for SocialMediaLink.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<SocialMediaLink> builder)
    {
        // Configure table name
        builder.ToTable("SocialMediaLink");

        // Configure primary key
        builder.HasKey(s => s.Id);

        // Configure required properties
        builder.Property(s => s.SocialMediaUrl).IsRequired();
        builder.Property(s => s.Network).IsRequired();
    }
}