using Domain.Entities;
using Shared.DTOs.Requests.Experience;

namespace BL.Interfaces.Experiences;

/// <summary>
///     Defines business operations for managing developer experiences.
/// </summary>
/// <remarks>
///     Provides comprehensive methods for creating, retrieving, updating, and deleting developer experiences
///     with built-in access control mechanisms.
/// </remarks>
public interface IExperienceManager
{
    /// <summary>
    ///     Adds a new experience to a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="dto">Data transfer object containing the experience details to create.</param>
    /// <param name="externalUserId">The external user identifier performing the action.</param>
    /// <returns>The newly created developer experience.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to add an experience.</exception>
    Task<Experience> AddExperienceAsync(string developerId, CreateExperienceRequest dto, string externalUserId);

    /// <summary>
    ///     Updates an existing developer experience.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="experienceId">The unique identifier of the experience to update.</param>
    /// <param name="dto">Data transfer object containing the updated experience information.</param>
    /// <param name="externalUserId">The external user identifier performing the update.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to update the experience.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the experience is not found.</exception>
    Task UpdateExperienceAsync(string developerId, string experienceId, UpdateExperienceRequest dto,
        string externalUserId);

    /// <summary>
    ///     Deletes a specific developer experience.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="experienceId">The unique identifier of the experience to delete.</param>
    /// <param name="externalUserId">The external user identifier performing the deletion.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to delete the experience.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the experience is not found.</exception>
    Task DeleteExperienceAsync(string developerId, string experienceId, string externalUserId);

    /// <summary>
    ///     Retrieves a specific developer experience by its identifier.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="experienceId">The unique identifier of the experience to retrieve.</param>
    /// <param name="externalUserId">The external user identifier performing the retrieval.</param>
    /// <returns>The requested developer experience.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to view the experience.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the experience is not found.</exception>
    Task<Experience> GetExperienceAsync(string developerId, string experienceId, string externalUserId);

    /// <summary>
    ///     Retrieves all experiences for a specific developer.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="externalUserId">The external user identifier performing the retrieval.</param>
    /// <returns>A collection of developer experiences.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to view the experiences.</exception>
    Task<IEnumerable<Experience>> GetAllExperiencesAsync(string developerId, string externalUserId);
}