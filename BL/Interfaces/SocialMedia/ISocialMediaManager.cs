using Domain.Entities;
using Domain.Enums;
using Shared.DTOs.Requests.SocialMediaLink;

namespace BL.Interfaces.SocialMedia;

/// <summary>
///     Defines business operations for managing a developer's social media links.
/// </summary>
/// <remarks>
///     Provides methods for creating, retrieving, and deleting social media links
///     with integrated access control mechanisms.
/// </remarks>
public interface ISocialMediaManager
{
   /// <summary>
   ///     Adds a new social media link to a developer's profile.
   /// </summary>
   /// <param name="developerId">The unique identifier of the developer.</param>
   /// <param name="dto">Data transfer object containing the social media link details to create.</param>
   /// <param name="externalUserId">The external user identifier performing the action.</param>
   /// <returns>The newly created social media link.</returns>
   /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to add a social media link.</exception>
   /// <exception cref="ArgumentException">Thrown when the provided link details are invalid.</exception>
   Task<SocialMediaLink> AddSocialMediaLinkAsync(string developerId, CreateSocialMediaLinkRequest dto,
        string externalUserId);

   /// <summary>
   ///     Deletes a specific social media link from a developer's profile.
   /// </summary>
   /// <param name="developerId">The unique identifier of the developer.</param>
   /// <param name="network">The social media network to remove.</param>
   /// <param name="externalUserId">The external user identifier performing the deletion.</param>
   /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to delete the social media link.</exception>
   /// <exception cref="KeyNotFoundException">Thrown when the social media link is not found.</exception>
   Task DeleteSocialMediaLinkAsync(string developerId, SocialMediaNetwork network, string externalUserId);

   /// <summary>
   ///     Retrieves all social media links for a specific developer.
   /// </summary>
   /// <param name="developerId">The unique identifier of the developer.</param>
   /// <param name="externalUserId">The external user identifier performing the retrieval.</param>
   /// <returns>A collection of social media links associated with the developer.</returns>
   /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to view the social media links.</exception>
   Task<IEnumerable<SocialMediaLink>> GetAllSocialMediaLinksAsync(string developerId, string externalUserId);
}