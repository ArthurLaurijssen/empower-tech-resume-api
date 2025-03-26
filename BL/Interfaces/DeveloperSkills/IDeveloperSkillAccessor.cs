using Domain.Entities;

namespace BL.Interfaces.DeveloperSkills;

/// <summary>
///     Provides access control mechanisms for developer skills.
/// </summary>
/// <remarks>
///     Defines methods for validating access to developer skills based on user permissions.
/// </remarks>
public interface IDeveloperSkillAccessor
{
    /// <summary>
    ///     Validates access to a specific developer skill.
    /// </summary>
    /// <param name="skill">The developer skill to check access for. Can be null.</param>
    /// <param name="developerId">The unique identifier of the developer associated with the skill.</param>
    /// <param name="externalUserId">The external user identifier performing the access check.</param>
    /// <returns>
    ///     The validated developer skill if access is permitted.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to access the skill.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the skill is not found.</exception>
    Task<DeveloperSkill> CheckAccessAsync(DeveloperSkill? skill, string developerId, string externalUserId);
}