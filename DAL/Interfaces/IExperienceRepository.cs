using Domain.Entities;

namespace DAL.Interfaces;

/// <summary>
///     Defines the contract for Experience entity data access operations.
///     Provides methods for retrieving, creating, updating, and deleting experience records
///     with various loading strategies for related entities.
/// </summary>
public interface IExperienceRepository
{
    /// <summary>
    ///     Retrieves an experience by ID with its associated developer and change tracking enabled.
    /// </summary>
    /// <param name="id">The unique identifier of the experience</param>
    /// <returns>The experience entity with its developer if found; otherwise, null</returns>
    Task<Experience?> GetByIdWithDeveloperAsync(Guid id);

    /// <summary>
    ///     Retrieves an experience by ID with its associated developer without change tracking.
    ///     Useful for read-only scenarios requiring developer information.
    /// </summary>
    /// <param name="id">The unique identifier of the experience</param>
    /// <returns>The experience entity with its developer if found; otherwise, null</returns>
    Task<Experience?> GetByIdWithDeveloperNoTrackingAsync(Guid id);

    /// <summary>
    ///     Retrieves all experiences for a specific developer without change tracking.
    ///     Useful for getting a developer's complete experience history in read-only scenarios.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <returns>A collection of experience entities associated with the specified developer</returns>
    Task<IEnumerable<Experience>> GetAllByDeveloperIdNoTrackingAsync(Guid developerId);

    /// <summary>
    ///     Registers a new experience in the system.
    /// </summary>
    /// <param name="experience">The experience entity to register</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RegisterAsync(Experience experience);

    /// <summary>
    ///     Persists all changes made to the experience entities in the database.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation</returns>
    Task SaveChangesAsync();

    /// <summary>
    ///     Checks if an experience with the specified ID exists in the system.
    /// </summary>
    /// <param name="id">The unique identifier of the experience to check</param>
    /// <returns>True if the experience exists; otherwise, false</returns>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    ///     Deletes an experience from the system using its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the experience to delete</param>
    /// <returns>A task representing the asynchronous delete operation</returns>
    Task DeleteByIdAsync(Guid id);
}