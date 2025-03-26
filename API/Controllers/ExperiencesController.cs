using API.Responses;
using AutoMapper;
using BL.Interfaces.Experiences;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Response.Experience;

namespace API.Controllers;

/// <summary>
///     Controller for retrieving collections of developer experiences.
///     Provides endpoints for accessing all experience records for a developer.
/// </summary>
/// <remarks>
///     Requires JWT authentication by default, with anonymous access allowed for GET operations.
///     This controller focuses on read operations for multiple experience records.
/// </remarks>
[ApiController]
[Route("api/developer/{developerId}/experiences")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ExperiencesController : AuthorizedApiController
{
    private readonly IExperienceManager _experienceManager;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the ExperiencesController.
    /// </summary>
    /// <param name="experienceManager">Service for managing experience operations</param>
    /// <param name="mapper">AutoMapper instance for DTO mappings</param>
    public ExperiencesController(IExperienceManager experienceManager, IMapper mapper)
    {
        _experienceManager = experienceManager;
        _mapper = mapper;
    }

    /// <summary>
    ///     Retrieves all experience entries for a specific developer.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer whose experiences to retrieve</param>
    /// <returns>API response containing a collection of the developer's experiences</returns>
    /// <remarks>
    ///     This endpoint provides access to all experience records associated with a developer.
    ///     The response includes basic experience information without additional details.
    /// </remarks>
    /// <response code="200">Successfully retrieved the list of experiences</response>
    /// <response code="401">User is not authenticated or token is invalid</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ExperienceResponse>>>> GetAllExperiences(string developerId)
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Retrieve all experiences for the developer
        var experiences = await _experienceManager.GetAllExperiencesAsync(developerId, UserId);

        // Map domain entities to DTO responses
        var response = _mapper.Map<IEnumerable<ExperienceResponse>>(experiences);

        return Ok(new ApiResponse<IEnumerable<ExperienceResponse>>
        {
            Data = response,
            Message = "Successfully retrieved experiences",
            Success = true
        });
    }
}