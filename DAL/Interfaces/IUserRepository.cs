using Domain.User;

namespace DAL.Interfaces;

/// <summary>
///     User repository interface.
///     Defines methods for interacting with user data.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    ///     Retrieves a user by their external (Auth0) ID.
    /// </summary>
    /// <param name="externalId">The external identifier (Auth0 sub claim)</param>
    Task<User?> GetByExternalIdAsync(string externalId);

    /// <summary>
    ///     Retrieves a user by their external (Auth0) ID.
    /// </summary>
    /// <param name="externalId">The external identifier (Auth0 sub claim)</param>
    Task<User?> GetByExternalIdIncludingPermissionsAsync(string externalId);

    /// <summary>
    ///     Retrieves a user by their internal ID.
    /// </summary>
    /// <param name="id">The internal unique identifier</param>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    ///     Registers a new user.
    /// </summary>
    /// <param name="user">The user to register</param>
    Task RegisterAsync(User user);

    /// <summary>
    ///     Updates an existing user.
    /// </summary>
    /// <param name="user">The user to update</param>
    Task UpdateAsync(User user);

    Task<List<User>> GetAllUsersWithSpecificPermissionAsync(string resource, string resourceId);

    /// <summary>
    ///     Persists changes to the database.
    /// </summary>
    Task SaveChangesAsync();
}