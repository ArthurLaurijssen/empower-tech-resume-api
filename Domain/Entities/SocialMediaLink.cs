using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
///     Represents a social media profile link for a developer.
///     This entity maintains the connection between a developer's profile and their
///     various social media presence across different platforms.
/// </summary>
public class SocialMediaLink : BaseEntity
{
    /// <summary>
    ///     Initializes a new instance of the SocialMediaLink class.
    ///     Required for Entity Framework Core.
    /// </summary>
    private SocialMediaLink()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the SocialMediaLink class with the specified parameters.
    ///     Validates the URL before creating the instance.
    /// </summary>
    /// <param name="socialMediaUrl">URL to the social media profile</param>
    /// <param name="network">Type of social media network</param>
    /// <exception cref="DomainValidationException">Thrown when URL is empty or whitespace</exception>
    private SocialMediaLink(string socialMediaUrl, SocialMediaNetwork network)
    {
        // Validate the social media URL
        if (string.IsNullOrWhiteSpace(socialMediaUrl))
            throw new DomainValidationException("URL cannot be empty");

        SocialMediaUrl = socialMediaUrl;
        Network = network;
    }

    /// <summary>
    ///     Gets the URL to the social media profile.
    /// </summary>
    public string SocialMediaUrl { get; private set; }

    /// <summary>
    ///     Gets the type of social media network this link represents.
    /// </summary>
    public SocialMediaNetwork Network { get; private set; }

    /// <summary>
    ///     Creates a new SocialMediaLink instance with the specified parameters.
    /// </summary>
    /// <param name="socialMediaUrl">URL to the social media profile</param>
    /// <param name="socialMediaNetworkName">Name of the social media network (case-insensitive)</param>
    /// <returns>A new instance of SocialMediaLink</returns>
    /// <exception cref="DomainValidationException">
    ///     Thrown when:
    ///     - URL is empty or whitespace
    ///     - The provided network name is not a valid SocialMediaNetwork enum value
    /// </exception>
    public static SocialMediaLink Create(string socialMediaUrl, string socialMediaNetworkName)
    {
        // Parse and validate the social media network from string
        if (!Enum.TryParse<SocialMediaNetwork>(socialMediaNetworkName, true, out var network))
            throw new DomainValidationException($"Invalid network: {socialMediaNetworkName}");

        return new SocialMediaLink(socialMediaUrl, network);
    }
}