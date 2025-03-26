using Domain.Entities;
using Shared.DTOs.Requests.Developer;

namespace BL.Interfaces.Developers;

/// <summary>
///     Defines business operations for managing developers, including creation, retrieval, update, and deletion.
/// </summary>
/// <remarks>
///     Provides a comprehensive set of methods for handling developer-related business logic,
///     with built-in access control through external user ID validation.
/// </remarks>
public interface IDeveloperManager
{
    /// <summary>
    ///     Creates a new developer with default values and grants permission to the creator.
    /// </summary>
    /// <param name="createdByUserId">The unique identifier of the user creating the developer.</param>
    /// <returns>The newly created developer with default configurations.</returns>
    /// <remarks>
    ///     This method initializes a new developer profile with standard default settings
    ///     and ensures the creator has appropriate permissions for the new profile.
    /// </remarks>
    Task<Developer> AddDefaultDeveloperWithPermissionToCreator(string createdByUserId);

    /// <summary>
    ///     Retrieves a collection of developers with basic information, filtered by the external user's access rights.
    /// </summary>
    /// <param name="externalUserId">The external user identifier used for access filtering.</param>
    /// <returns>A collection of developers with limited details that the user can access.</returns>
    /// <remarks>
    ///     Returns a lightweight version of developers, typically used for listing or overview purposes.
    ///     Restricts results based on the external user's permissions.
    /// </remarks>
    Task<IEnumerable<Developer>> GetAllBasicDevelopersAsync(string externalUserId);

    /// <summary>
    ///     Retrieves all developers with comprehensive information, including experiences, social links, and proficiencies.
    /// </summary>
    /// <param name="externalUserId">The external user identifier used for access filtering.</param>
    /// <returns>A collection of developers with full, detailed information.</returns>
    /// <remarks>
    ///     Provides an extensive view of developers, including in-depth profile details.
    ///     Access is restricted based on the external user's permissions.
    /// </remarks>
    Task<IEnumerable<Developer>> GetAllDevelopersWithDetailsAsync(string externalUserId);

    /// <summary>
    ///     Deletes a specific developer profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer to be deleted.</param>
    /// <param name="externalUserId">The external user identifier performing the deletion.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the developer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to delete the developer.</exception>
    /// <remarks>
    ///     Removes a developer profile from the system.
    ///     Requires appropriate authorization based on the external user's permissions.
    /// </remarks>
    Task DeleteDeveloper(string developerId, string externalUserId);

    /// <summary>
    ///     Retrieves a specific developer by their ID with basic information.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer to retrieve.</param>
    /// <returns>The developer with basic details.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the developer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks access to the developer's profile.</exception>
    Task<Developer> GetDeveloperByIdAsync(string developerId);

    /// <summary>
    ///     Retrieves a specific developer by their ID with comprehensive profile information.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer to retrieve.</param>
    /// <returns>The developer with full, detailed profile information.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the developer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks access to the developer's detailed profile.</exception>
    /// <remarks>
    ///     Provides an extensive view of the developer's profile, including experiences,
    ///     social links, proficiencies, and other in-depth information.
    /// </remarks>
    Task<Developer> GetDeveloperWithDetailsByIdAsync(string developerId);

    /// <summary>
    ///     Updates an existing developer's profile information.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer to update.</param>
    /// <param name="updateDeveloperDto">Data transfer object containing the updated developer information.</param>
    /// <param name="externalUserId">The external user identifier performing the update.</param>
    /// <returns>A task representing the asynchronous update operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the developer is not found.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the user lacks permission to update the developer's profile.</exception>
    /// <remarks>
    ///     Allows modification of an existing developer's profile.
    ///     Requires appropriate authorization and validates the update request.
    /// </remarks>
    Task UpdateDeveloperAsync(string developerId, UpdateDeveloperRequest updateDeveloperDto, string externalUserId);
}