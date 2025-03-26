namespace Domain.Common;

/// <summary>
///     Represents a base class for entities in the domain model, providing common properties and behavior.
/// </summary>
/// <remarks>
///     This abstract class serves as the foundation for all domain entities, ensuring consistent
///     identity management across the system. It implements a GUID-based identification pattern
///     which is suitable for distributed systems and microservices architecture.
/// </remarks>
public abstract class BaseEntity
{
    /// <summary>
    ///     Gets or sets the unique identifier for the entity.
    /// </summary>
    /// <value>
    ///     A globally unique identifier (GUID) that uniquely identifies the entity instance.
    ///     This property is typically set during entity creation and remains unchanged
    ///     throughout the entity's lifecycle.
    /// </value>
    public Guid Id { get; set; }
}