namespace Domain.Contracts;

/// <summary>
///     Represents a marker interface for aggregate roots in Domain-Driven Design (DDD).
/// </summary>
/// <remarks>
///     Aggregate roots are entities that maintain transactional consistency boundaries
///     and serve as the entry point to an aggregate. This interface helps identify
///     which entities serve as aggregate roots in the domain model.
/// </remarks>
public interface IAggregateRoot
{
    // Marker interface - no members required
}