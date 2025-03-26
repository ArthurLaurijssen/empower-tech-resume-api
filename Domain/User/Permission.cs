using Domain.Common;
using Domain.User.Enums;

namespace Domain.User;

/// <summary>
/// Represents a permission entity that defines access rights to specific resources.
/// This class handles both system-wide and resource-specific permissions through
/// the combination of resource identifiers and permission scopes.
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the Permission class.
    /// Required for Entity Framework Core.
    /// </summary>
    private Permission()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Permission class with specified parameters.
    /// </summary>
    /// <param name="resource">The type of resource this permission applies to</param>
    /// <param name="resourceId">Specific resource identifier, or null for all resources of the type</param>
    /// <param name="scope">The scope of the permission (All or Specific)</param>
    private Permission(string resource, string? resourceId, PermissionScope scope)
    {
        Resource = resource;
        ResourceId = resourceId;
        Scope = scope;
    }

    /// <summary>
    /// Gets the type of resource this permission applies to (e.g., "developer", "project").
    /// </summary>
    public string Resource { get; private set; }

    /// <summary>
    /// Gets the specific resource identifier this permission applies to.
    /// Null indicates the permission applies to all resources of the specified type.
    /// </summary>
    public string? ResourceId { get; private set; }

    /// <summary>
    /// Gets the scope of the permission, determining whether it applies
    /// to all resources of a type or only to specific instances.
    /// </summary>
    public PermissionScope Scope { get; private set; }

    /// <summary>
    /// Creates a permission that applies to all instances of a specific resource type.
    /// </summary>
    /// <param name="resource">The type of resource this permission applies to</param>
    /// <returns>A new Permission instance with 'All' scope</returns>
    /// <example>
    /// Create permission for all developers:
    /// <code>
    /// var allDevelopersPermission = Permission.CreateAllPermission("developer");
    /// </code>
    /// </example>
    public static Permission CreateAllPermission(string resource)
    {
        // Create permission with null resourceId to indicate it applies to all instances
        return new Permission(resource, null, PermissionScope.All);
    }

    /// <summary>
    /// Creates a permission that applies to a specific resource instance.
    /// </summary>
    /// <param name="resource">The type of resource this permission applies to</param>
    /// <param name="resourceId">The specific identifier of the resource instance</param>
    /// <returns>A new Permission instance with 'Specific' scope</returns>
    /// <example>
    /// Create permission for a specific developer:
    /// <code>
    /// var specificDeveloperPermission = Permission.CreateSpecificPermission("developer", "dev-123");
    /// </code>
    /// </example>
    public static Permission CreateSpecificPermission(string resource, string resourceId)
    {
        // Create permission for a specific resource instance
        return new Permission(resource, resourceId, PermissionScope.Specific);
    }
}