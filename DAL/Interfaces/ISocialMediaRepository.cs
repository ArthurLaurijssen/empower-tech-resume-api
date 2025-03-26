namespace DAL.Interfaces;

/// <summary>
///     Defines the contract for SocialMediaLink entity data access operations.
///     Provides methods for managing social media link records in the system.
/// </summary>
public interface ISocialMediaRepository
{
    /// <summary>
    ///     Persists all changes made to the social media link entities in the database.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation</returns>
    Task SaveChangesAsync();

    /// <summary>
    ///     Checks if a social media link with the specified ID exists in the system.
    /// </summary>
    /// <param name="id">The unique identifier of the social media link to check</param>
    /// <returns>True if the social media link exists; otherwise, false</returns>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    ///     Deletes a social media link from the system using its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the social media link to delete</param>
    /// <returns>A task representing the asynchronous delete operation</returns>
    Task DeleteByIdAsync(Guid id);
}