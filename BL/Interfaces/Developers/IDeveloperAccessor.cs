using Domain.Entities;

namespace BL.Interfaces.Developers;

/// <summary>
///     Provides methods for managing and validating developer access rights.
/// </summary>
/// <remarks>
///     This interface defines operations for checking and filtering developer access
///     based on external user identifiers, ensuring proper authorization and access control.
/// </remarks>
public interface IDeveloperAccessor
{
    /// <summary>
    ///     Checks and validates access for a specific developer.
    /// </summary>
    /// <param name="developer">The developer to be checked. Can be null.</param>
    /// <param name="developerId">The unique identifier of the developer.</param>
    /// <param name="externalUserId">The external user identifier for access validation.</param>
    /// <returns>
    ///     A <see cref="Task{Developer}" /> representing the asynchronous operation.
    ///     Returns the validated developer if access is granted.
    /// </returns>
    /// <remarks>
    ///     This method performs access validation for a developer based on the provided external user ID.
    ///     It can handle scenarios where the developer might be null or require additional access checks.
    /// </remarks>
    /// <exception cref="UnauthorizedAccessException">Thrown when access is not permitted.</exception>
    Task<Developer> CheckAccessAsync(Developer? developer, string developerId, string externalUserId);

    /// <summary>
    ///     Filters a collection of developers to return only those accessible to the specified external user.
    /// </summary>
    /// <param name="developers">The collection of developers to filter.</param>
    /// <param name="externalUserId">The external user identifier used for access filtering.</param>
    /// <remarks>
    ///     This method provides a way to restrict access to a subset of developers
    ///     based on the permissions of the external user.
    /// </remarks>
    Task<IEnumerable<Developer>> FilterAccessibleDevelopers(IEnumerable<Developer> developers, string externalUserId);
}