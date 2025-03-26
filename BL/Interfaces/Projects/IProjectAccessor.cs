using Domain.Entities;

namespace BL.Interfaces.Projects;

/// <summary>
///     Provides access control mechanisms for projects.
/// </summary>
/// <remarks>
///     Defines methods for validating access to projects based on user permissions.
/// </remarks>
public interface IProjectAccessor
{
    /// <summary>
    ///     Validates access to a specific project.
    /// </summary>
    /// <param name="project">The project to check access for. Can be null.</param>
    /// <param name="externalUserId">The external user identifier performing the access check.</param>
    /// <returns>
    ///     The validated project if access is permitted.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to access the project.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the project is not found.</exception>
    Task<Project> CheckAccessAsync(Project? project, string externalUserId);
}