using Domain.Entities;
using Shared.DTOs.Requests.Project;

namespace BL.Interfaces.Projects;

/// <summary>
///     Defines business operations for managing projects associated with developer skills.
/// </summary>
/// <remarks>
///     Provides methods for creating, updating, and deleting projects with integrated access control.
/// </remarks>
public interface IProjectManager
{
    /// <summary>
    ///     Adds a new project to a specific developer skill.
    /// </summary>
    /// <param name="developerSkill">The unique identifier of the developer skill.</param>
    /// <param name="projectDto">Data transfer object containing the project details to create.</param>
    /// <param name="externalUserId">The external user identifier performing the action.</param>
    /// <returns>The newly created project.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to add a project.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the developer skill is not found.</exception>
    Task<Project> AddProjectToDeveloperSkillAsync(string developerSkill, CreateProjectRequest projectDto,
        string externalUserId);

    /// <summary>
    ///     Updates an existing project.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project to update.</param>
    /// <param name="projectDto">Data transfer object containing the updated project information.</param>
    /// <param name="externalUserId">The external user identifier performing the update.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to update the project.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the project is not found.</exception>
    Task UpdateProjectAsync(string projectId, UpdateProjectRequest projectDto, string externalUserId);

    /// <summary>
    ///     Deletes a specific project.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project to delete.</param>
    /// <param name="externalUserId">The external user identifier performing the deletion.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to delete the project.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the project is not found.</exception>
    Task DeleteProjectAsync(string projectId, string externalUserId);
}