namespace DAL.Interfaces;

/// <summary>
///     Defines the contract for Permission entity data access operations.
///     Provides methods for managing permissions in the system, particularly
///     focused on bulk deletion operations for specific resources.
/// </summary>
public interface IPermissionRepository
{
    /// <summary>
    ///     Deletes all permissions associated with a specific resource instance.
    ///     This operation removes all permissions that match both the resource type and identifier.
    /// </summary>
    /// <param name="resourceName">The name of the resource type (e.g., "developer", "project")</param>
    /// <param name="resourceIdentifier">The specific identifier of the resource instance</param>
    /// <returns>A task representing the asynchronous delete operation</returns>
    /// <example>
    ///     Delete all permissions for a specific developer:
    ///     <code>
    /// await DeleteAllAsync("developer", "dev-123");
    /// </code>
    /// </example>
    Task DeleteAllAsync(string resourceName, string resourceIdentifier);

    /// <summary>
    ///     Persists all changes made to the permission entities in the database.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation</returns>
    Task SaveChangesAsync();
}