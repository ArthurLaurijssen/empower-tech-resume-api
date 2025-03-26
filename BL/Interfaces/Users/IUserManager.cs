using Domain.User;

namespace BL.Interfaces.Users;

/// <summary>
///     Defines operations for managing user accounts and their associated permissions.
/// </summary>
/// <remarks>
///     Provides methods to retrieve existing users or create new user accounts
///     with optional permission loading.
/// </remarks>
public interface IUserManager
{
    /// <summary>
    ///     Retrieves an existing user or creates a new user account if one does not exist.
    /// </summary>
    /// <param name="externalId">The unique external identifier of the user.</param>
    /// <returns>The retrieved or newly created user account.</returns>
    /// <exception cref="ArgumentException">Thrown when the external identifier is invalid.</exception>
    Task<User> GetOrCreateUserAsync(string externalId);

    /// <summary>
    ///     Retrieves an existing user or creates a new user account,
    ///     with the option to include the user's permissions.
    /// </summary>
    /// <param name="externalId">The unique external identifier of the user.</param>
    /// <returns>The retrieved or newly created user account with loaded permissions.</returns>
    /// <exception cref="ArgumentException">Thrown when the external identifier is invalid.</exception>
    Task<User> GetOrCreateUserIncludingPermissionsAsync(string externalId);
}