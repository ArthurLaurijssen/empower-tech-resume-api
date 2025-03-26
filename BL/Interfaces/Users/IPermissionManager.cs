namespace BL.Interfaces.Users;

/// <summary>
///     Defines operations for managing user permissions across different resources.
/// </summary>
/// <remarks>
///     Provides methods to grant, modify, and revoke user permissions
///     for specific resources and resource identifiers.
/// </remarks>
public interface IPermissionManager
{
    /// <summary>
    ///     Grants a user all permissions for a specific resource.
    /// </summary>
    /// <param name="externalUserId">The unique external identifier of the user.</param>
    /// <param name="resourceName">The name of the resource to grant permissions for.</param>
    /// <exception cref="ArgumentException">Thrown when the resource name or user identifier is invalid.</exception>
    Task GiveUserAllPermissionAsync(string externalUserId, string resourceName);

    /// <summary>
    ///     Grants a user specific permissions for a resource with a particular identifier.
    /// </summary>
    /// <param name="externalUserId">The unique external identifier of the user.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <param name="resourceIdentifier">The specific identifier of the resource.</param>
    /// <exception cref="ArgumentException">Thrown when the resource name, user identifier, or resource identifier is invalid.</exception>
    Task GiveUserSpecificPermissionAsync(string externalUserId, string resourceName, string resourceIdentifier);

    /// <summary>
    ///     Removes a specific permission from all users for a given resource identifier.
    /// </summary>
    /// <param name="resourceName">The name of the resource.</param>
    /// <param name="resourceIdentifier">The specific identifier of the resource.</param>
    /// <exception cref="ArgumentException">Thrown when the resource name or resource identifier is invalid.</exception>
    Task RemoveSpecificPermissionFromAllUsersAsync(string resourceName, string resourceIdentifier);
}