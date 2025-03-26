using Domain.Entities;

namespace DAL.Interfaces;

/// <summary>
///     Defines the contract for Project entity data access operations.
///     Provides methods for retrieving, creating, updating, and deleting project records
///     with various loading strategies for related entities.
/// </summary>
public interface IProjectRepository
{
    /// <summary>
    ///     Retrieves a project by ID with all related details and change tracking enabled.
    ///     Includes associated developer skills and their details.
    /// </summary>
    /// <param name="id">The unique identifier of the project</param>
    /// <returns>The project entity with all related details if found; otherwise, null</returns>
    Task<Project?> GetByIdWithDetailsAsync(Guid id);

    /// <summary>
    ///     Retrieves a project by ID with all related details without change tracking.
    ///     Includes associated developer skills and their details.
    ///     Useful for read-only scenarios requiring complete project information.
    /// </summary>
    /// <param name="id">The unique identifier of the project</param>
    /// <returns>The project entity with all related details if found; otherwise, null</returns>
    Task<Project?> GetByIdWithDetailsNoTrackingAsync(Guid id);

    /// <summary>
    ///     Retrieves all projects associated with a specific developer skill without change tracking.
    ///     Useful for viewing all projects that utilize a particular skill.
    /// </summary>
    /// <param name="skillId">The unique identifier of the developer skill</param>
    /// <returns>A collection of project entities associated with the specified skill</returns>
    Task<IEnumerable<Project>> GetAllByDeveloperSkillIdNoTrackingAsync(Guid skillId);

    /// <summary>
    ///     Registers a new project in the system.
    /// </summary>
    /// <param name="project">The project entity to register</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RegisterAsync(Project project);

    /// <summary>
    ///     Persists all changes made to the project entities in the database.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation</returns>
    Task SaveChangesAsync();

    /// <summary>
    ///     Checks if a project with the specified ID exists in the system.
    /// </summary>
    /// <param name="id">The unique identifier of the project to check</param>
    /// <returns>True if the project exists; otherwise, false</returns>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    ///     Deletes a project from the system using its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the project to delete</param>
    /// <returns>A task representing the asynchronous delete operation</returns>
    Task DeleteByIdAsync(Guid id);
}