using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
///     Base controller class that provides authorization-related functionality for API endpoints.
///     Inherits from ControllerBase and serves as an abstract base for authorized controllers.
/// </summary>
public abstract class AuthorizedApiController : ControllerBase
{
    /// <summary>
    ///     Gets the unique identifier of the currently authenticated user.
    /// </summary>
    /// <returns>
    ///     The user's ID from the NameIdentifier claim if present; otherwise, returns an empty string.
    /// </returns>
    /// <remarks>
    ///     This property safely extracts the user ID from claims without throwing exceptions if claims are missing.
    ///     It should be used in derived controllers to access the current user's identity.
    /// </remarks>
    protected string UserId =>
        // Search for the NameIdentifier claim in the current User's claims
        // If found, return its Value; if not found (?.), return empty string (?? string.Empty)
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
}