using Domain.Entities;

namespace DAL.Interfaces;

/// <summary>
///     Defines the contract for Developer entity data access operations.
///     Provides methods for retrieving, creating, updating, and deleting developer records
///     with various loading strategies for related entities and change tracking options.
/// </summary>
public interface IDeveloperRepository
{
    /// <summary>
    ///     Retrieves a developer by ID with change tracking without related entities.
    /// </summary>
    /// <param name="id">The unique identifier of the developer</param>
    /// <returns>The developer entity if found; otherwise, null</returns>
    Task<Developer?> GetByIdAsync(Guid id);

    /// <summary>
    ///     Retrieves a developer by ID with their social media links and change tracking enabled.
    /// </summary>
    /// <param name="id">The unique identifier of the developer</param>
    /// <returns>The developer entity with social media links if found; otherwise, null</returns>
    Task<Developer?> GetByIdWithSocialMediaLinksAsync(Guid id);

    /// <summary>
    ///     Retrieves a developer by ID without change tracking.
    ///     This is useful for read-only scenarios where the entity won't be modified.
    /// </summary>
    /// <param name="id">The unique identifier of the developer</param>
    /// <returns>The developer entity if found; otherwise, null</returns>
    Task<Developer?> GetByIdNoTrackingAsync(Guid id);

    /// <summary>
    ///     Retrieves a developer by ID with all related entities and change tracking enabled.
    ///     Includes DeveloperSkills, Experiences, and SocialMediaLinks.
    /// </summary>
    /// <param name="id">The unique identifier of the developer</param>
    /// <returns>The developer entity with all related entities if found; otherwise, null</returns>
    Task<Developer?> GetByIdWithDetailsAsync(Guid id);

    /// <summary>
    ///     Retrieves a developer by ID with all related entities without change tracking.
    ///     Includes DeveloperSkills, Experiences, and SocialMediaLinks.
    ///     Useful for read-only scenarios requiring full entity details.
    /// </summary>
    /// <param name="id">The unique identifier of the developer</param>
    /// <returns>The developer entity with all related entities if found; otherwise, null</returns>
    Task<Developer?> GetByIdWithDetailsNoTrackingAsync(Guid id);

    /// <summary>
    ///     Retrieves all developers without related entities and with change tracking enabled.
    /// </summary>
    /// <returns>A collection of all developer entities</returns>
    Task<IEnumerable<Developer>> GetAllAsync();

    /// <summary>
    ///     Retrieves all developers without related entities and without change tracking.
    ///     Useful for read-only scenarios requiring all developers.
    /// </summary>
    /// <returns>A collection of all developer entities</returns>
    Task<IEnumerable<Developer>> GetAllNoTrackingAsync();

    /// <summary>
    ///     Retrieves all developers with their related entities and change tracking enabled.
    ///     Includes DeveloperSkills, Experiences, and SocialMediaLinks.
    /// </summary>
    /// <returns>A collection of all developer entities with their related entities</returns>
    Task<IEnumerable<Developer>> GetAllWithDetailsAsync();

    /// <summary>
    ///     Retrieves all developers with their related entities without change tracking.
    ///     Includes DeveloperSkills, Experiences, and SocialMediaLinks.
    ///     Useful for read-only scenarios requiring full entity details.
    /// </summary>
    /// <returns>A collection of all developer entities with their related entities</returns>
    Task<IEnumerable<Developer>> GetAllWithDetailsNoTrackingAsync();

    /// <summary>
    ///     Registers a new developer in the system.
    /// </summary>
    /// <param name="developer">The developer entity to register</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RegisterAsync(Developer developer);

    /// <summary>
    ///     Removes an existing developer from the system.
    /// </summary>
    /// <param name="developer">The developer entity to remove</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RemoveAsync(Developer developer);

    /// <summary>
    ///     Persists all changes made to the developer entities in the database.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation</returns>
    Task SaveChangesAsync();

    /// <summary>
    ///     Checks if a developer with the specified ID exists in the system.
    /// </summary>
    /// <param name="id">The unique identifier of the developer to check</param>
    /// <returns>True if the developer exists; otherwise, false</returns>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    ///     Deletes a developer from the system using their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the developer to delete</param>
    /// <returns>A task representing the asynchronous delete operation</returns>
    Task DeleteByIdAsync(Guid id);
}