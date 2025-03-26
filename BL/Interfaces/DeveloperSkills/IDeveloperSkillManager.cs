using Domain.Entities;
using Shared.DTOs.Requests.DeveloperSkill;

namespace BL.Interfaces.DeveloperSkills;

/// <summary>
///     Defines business operations for managing developer skills.
/// </summary>
/// <remarks>
///     Provides comprehensive methods for creating, retrieving, updating, and deleting developer skills
///     with built-in access control mechanisms.
/// </remarks>
public interface IDeveloperSkillManager
{
    /// <summary>
    ///     Adds a new skill to a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="developerSkillDto">Data transfer object containing the skill details to create.</param>
    /// <param name="externalUserId">The external user identifier performing the action.</param>
    /// <returns>The newly created developer skill.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to add a skill.</exception>
    Task<DeveloperSkill> AddDeveloperSkillAsync(string developerId, CreateDeveloperSkillRequest developerSkillDto,
        string externalUserId);

    /// <summary>
    ///     Updates an existing developer skill.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="developerSkillId">The unique identifier of the skill to update.</param>
    /// <param name="developerSkillDto">Data transfer object containing the updated skill information.</param>
    /// <param name="externalUserId">The external user identifier performing the update.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to update the skill.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the skill is not found.</exception>
    Task UpdateDeveloperSkillAsync(string developerId, string developerSkillId,
        UpdateDeveloperSkillRequest developerSkillDto,
        string externalUserId);

    /// <summary>
    ///     Deletes a specific developer skill.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="developerSkillId">The unique identifier of the skill to delete.</param>
    /// <param name="externalUserId">The external user identifier performing the deletion.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to delete the skill.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the skill is not found.</exception>
    Task DeleteDeveloperSkillAsync(string developerId, string developerSkillId, string externalUserId);

    /// <summary>
    ///     Retrieves a specific developer skill by its identifier.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="developerSkillId">The unique identifier of the skill to retrieve.</param>
    /// <param name="externalUserId">The external user identifier performing the retrieval.</param>
    /// <returns>The requested developer skill.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to view the skill.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the skill is not found.</exception>
    Task<DeveloperSkill> GetDeveloperSkillAsync(string developerId, string developerSkillId, string externalUserId);

    /// <summary>
    ///     Retrieves a specific developer skill with associated projects.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="developerSkillId">The unique identifier of the skill to retrieve.</param>
    /// <param name="externalUserId">The external user identifier performing the retrieval.</param>
    /// <returns>The developer skill with its associated projects.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to view the skill.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the skill is not found.</exception>
    Task<DeveloperSkill> GetDeveloperSkillWithProjectsAsync(string developerId, string developerSkillId,
        string externalUserId);

    /// <summary>
    ///     Retrieves all developer skills with their associated projects, without tracking changes.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="externalUserId">The external user identifier performing the retrieval.</param>
    /// <returns>A collection of developer skills with associated projects.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to view the skills.</exception>
    Task<IEnumerable<DeveloperSkill>> GetAllDeveloperSkillsWithProjectNoTrackingAsync(string developerId,
        string externalUserId);

    /// <summary>
    ///     Retrieves all developer skills without tracking changes.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="externalUserId">The external user identifier performing the retrieval.</param>
    /// <returns>A collection of developer skills.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to view the skills.</exception>
    Task<IEnumerable<DeveloperSkill>> GetAllDeveloperSkillsNoTrackingAsync(string developerId, string externalUserId);
}