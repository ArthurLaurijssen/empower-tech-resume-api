using Domain.Common;
using Domain.Exceptions;
using Domain.User.Enums;

namespace Domain.User;

/// <summary>
/// Represents a user entity within the system with associated permissions.
/// This class manages user identity and their access rights through a collection
/// of permissions that control access to various resources.
/// </summary>
public class User : BaseEntity
{
    // Private backing field for permissions collection using C# 12 collection expression syntax
    private readonly List<Permission> _permissions = [];

    /// <summary>
    /// Initializes a new instance of the User class.
    /// Required for Entity Framework Core.
    /// </summary>
    private User()
    {
    }

    /// <summary>
    /// Initializes a new instance of the User class with specified external identifier.
    /// </summary>
    /// <param name="externalId">External identifier for the user (e.g., from authentication provider)</param>
    private User(string externalId)
    {
        ExternalId = externalId;
    }

    /// <summary>
    /// Gets a read-only collection of permissions associated with this user.
    /// This provides access to the user's permissions while preventing external modification.
    /// </summary>
    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    /// <summary>
    /// Gets the external identifier for this user, typically provided by an authentication service.
    /// </summary>
    public string ExternalId { get; private set; }

    /// <summary>
    /// Creates a new User instance with the specified external identifier.
    /// </summary>
    /// <param name="externalId">External identifier for the user</param>
    /// <returns>A new instance of User</returns>
    /// <exception cref="DomainValidationException">Thrown when external ID is empty or whitespace</exception>
    public static User Create(string externalId)
    {
        // Validate external ID
        if (string.IsNullOrWhiteSpace(externalId))
            throw new DomainValidationException("External ID cannot be empty");

        return new User(externalId);
    }

    /// <summary>
    /// Adds a permission to the user's permission collection.
    /// </summary>
    /// <param name="permission">The permission to add to the user</param>
    public void AddPermission(Permission permission)
    {
        _permissions.Add(permission);
    }

    /// <summary>
    /// Removes a specific permission from the user's permission collection.
    /// Only removes permissions with Specific scope matching the provided resource and resourceId.
    /// </summary>
    /// <param name="resource">The resource type to remove permission for</param>
    /// <param name="resourceId">The specific resource identifier to remove permission for</param>
    public void RemovePermission(string resource, string resourceId)
    {
        // Find and remove specific permission matching criteria
        var permission = _permissions
            .FirstOrDefault(p =>
                p.Resource == resource &&
                p.ResourceId == resourceId &&
                p.Scope == PermissionScope.Specific);

        if (permission != null)
            _permissions.Remove(permission);
    }

    /// <summary>
    /// Checks if the user has permission to access a specific resource.
    /// </summary>
    /// <param name="resource">The resource type to check permission for</param>
    /// <param name="resourceId">Optional specific resource identifier to check permission for</param>
    /// <returns>
    /// True if the user has permission, which can be either:
    /// - A matching All scope permission for the resource type
    /// - A matching Specific scope permission for the exact resource
    /// False otherwise
    /// </returns>
    public bool HasPermission(string resource, string? resourceId = null)
    {
        // Check for wildcard permission first (resource.*)
        if (_permissions.Any(p =>
                p.Resource == resource &&
                p.Scope == PermissionScope.All))
            return true;

        // If resourceId is provided, check for specific permission
        if (resourceId != null)
            return _permissions.Any(p =>
                p.Resource == resource &&
                p.Scope == PermissionScope.Specific &&
                p.ResourceId == resourceId);

        return false;
    }
}