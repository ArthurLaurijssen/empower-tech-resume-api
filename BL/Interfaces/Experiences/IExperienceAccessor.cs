using Domain.Entities;

namespace BL.Interfaces.Experiences;

/// <summary>
///     Provides access control mechanisms for developer experiences.
/// </summary>
/// <remarks>
///     Defines methods for validating access to developer experiences based on user permissions.
/// </remarks>
public interface IExperienceAccessor
{
    /// <summary>
    ///     Validates access to a specific developer experience.
    /// </summary>
    /// <param name="experience">The developer experience to check access for. Can be null.</param>
    /// <param name="developerId">The unique identifier of the developer associated with the experience.</param>
    /// <param name="externalUserId">The external user identifier performing the access check.</param>
    /// <returns>
    ///     The validated developer experience if access is permitted.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to access the experience.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the experience is not found.</exception>
    Task<Experience> CheckAccessAsync(Experience? experience, string developerId, string externalUserId);
}