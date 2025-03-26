using BL.Interfaces.Developers;
using BL.Interfaces.SocialMedia;
using DAL.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Shared.DTOs.Requests.SocialMediaLink;

namespace BL.Managers.SocialMedia;

/// <inheritdoc cref="ISocialMediaManager" />
public class SocialMediaManager(
    IDeveloperRepository developerRepository,
    ISocialMediaRepository socialMediaRepository,
    IDeveloperAccessor developerAccessor) : ISocialMediaManager
{
    /// <inheritdoc />
    public async Task<SocialMediaLink> AddSocialMediaLinkAsync(string developerId, CreateSocialMediaLinkRequest dto,
        string externalUserId)
    {
        // Parse the developer ID to a GUID
        var guidId = ParseGuid(developerId);

        // Retrieve the developer with their social media links
        var developer = await developerRepository.GetByIdWithSocialMediaLinksAsync(guidId);

        // Validate access to the developer
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Create a new social media link
        var link = SocialMediaLink.Create(
            dto.SocialMediaUrl,
            dto.SocialMediaNetworkName);

        // Add the link to the developer's profile
        developer.AddSocialMediaLink(link);

        // Save changes
        await developerRepository.SaveChangesAsync();

        return link;
    }

    /// <inheritdoc />
    public async Task DeleteSocialMediaLinkAsync(string developerId, SocialMediaNetwork network, string externalUserId)
    {
        // Parse the developer ID to a GUID
        var guidId = ParseGuid(developerId);

        // Retrieve the developer with their social media links
        var developer = await developerRepository.GetByIdWithSocialMediaLinksAsync(guidId);

        // Validate access to the developer
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Find the social media link for the specified network
        var socialMediaLink = developer.SocialMediaLinks.FirstOrDefault(x => x.Network == network);

        // Delete the link if found, otherwise throw an exception
        if (socialMediaLink != null)
            await socialMediaRepository.DeleteByIdAsync(socialMediaLink.Id);
        else
            throw new KeyNotFoundException(
                $"Social media link for network: {network} and developer {developerId} not found");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SocialMediaLink>> GetAllSocialMediaLinksAsync(string developerId,
        string externalUserId)
    {
        // Parse the developer ID to a GUID
        var guidId = ParseGuid(developerId);

        // Retrieve the developer with their social media links
        var developer = await developerRepository.GetByIdWithSocialMediaLinksAsync(guidId);

        // Validate access to the developer
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Return the developer's social media links
        return developer.SocialMediaLinks.AsEnumerable();
    }

    /// <summary>
    ///     Parses a string identifier into a GUID.
    /// </summary>
    /// <param name="id">The string identifier to parse.</param>
    /// <returns>A valid GUID representation of the identifier.</returns>
    /// <exception cref="FormatException">Thrown when the identifier cannot be parsed to a GUID.</exception>
    private static Guid ParseGuid(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new FormatException($"Invalid ID format: {id}");
        return guid;
    }
}