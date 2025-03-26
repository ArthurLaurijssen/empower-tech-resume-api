using Domain.Entities;

namespace DAL.Interfaces;

/// <summary>
///     Defines the contract for DeveloperSkill entity data access operations.
///     Provides methods for retrieving, creating, updating, and deleting developer skill records
///     with various loading strategies for related entities.
/// </summary>
public interface IDeveloperSkillRepository
{
    /// <summary>
    ///     Retrieves a developer skill by ID with its associated developer and change tracking enabled.
    /// </summary>
    /// <param name="id">The unique identifier of the developer skill</param>
    /// <returns>The developer skill entity with its developer if found; otherwise, null</returns>
    Task<DeveloperSkill?> GetByIdWithDeveloperAsync(Guid id);

    /// <summary>
    ///     Retrieves a developer skill by ID with its associated developer without change tracking.
    ///     Useful for read-only scenarios requiring developer information.
    /// </summary>
    /// <param name="id">The unique identifier of the developer skill</param>
    /// <returns>The developer skill entity with its developer if found; otherwise, null</returns>
    Task<DeveloperSkill?> GetByIdWithDeveloperNoTrackingAsync(Guid id);

    /// <summary>
    ///     Retrieves a developer skill by ID with its associated developer and projects without change tracking.
    ///     Includes both the developer and related projects for comprehensive skill details.
    /// </summary>
    /// <param name="id">The unique identifier of the developer skill</param>
    /// <returns>The developer skill entity with its developer and projects if found; otherwise, null</returns>
    Task<DeveloperSkill?> GetByIdWithDeveloperAndProjectsNoTrackingAsync(Guid id);

    /// <summary>
    ///     Retrieves all skills for a specific developer without change tracking.
    ///     Useful for getting a developer's complete skill set in read-only scenarios.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <returns>A collection of developer skill entities associated with the specified developer</returns>
    Task<IEnumerable<DeveloperSkill>> GetAllByDeveloperIdNoTrackingAsync(Guid developerId);

    /// <summary>
    ///     Retrieves all skills for a specific developer including related projects without change tracking.
    ///     Provides a comprehensive view of a developer's skills and the projects where they were applied.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <returns>A collection of developer skill entities with their related projects for the specified developer</returns>
    Task<IEnumerable<DeveloperSkill>> GetAllByDeveloperIdWithProjectsNoTrackingAsync(Guid developerId);

    /// <summary>
    ///     Registers a new developer skill in the system.
    /// </summary>
    /// <param name="developerSkill">The developer skill entity to register</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RegisterAsync(DeveloperSkill developerSkill);

    /// <summary>
    ///     Persists all changes made to the developer skill entities in the database.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation</returns>
    Task SaveChangesAsync();

    /// <summary>
    ///     Checks if a developer skill with the specified ID exists in the system.
    /// </summary>
    /// <param name="id">The unique identifier of the developer skill to check</param>
    /// <returns>True if the developer skill exists; otherwise, false</returns>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    ///     Deletes a developer skill from the system using its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the developer skill to delete</param>
    /// <returns>A task representing the asynchronous delete operation</returns>
    Task DeleteByIdAsync(Guid id);
}