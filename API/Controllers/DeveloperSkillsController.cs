using API.Responses;
using AutoMapper;
using BL.Interfaces.DeveloperSkills;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Response.DeveloperSkill;

namespace API.Controllers;

/// <summary>
///     Controller for managing collections of developer skills.
///     Provides endpoints for retrieving both basic and detailed skill information.
/// </summary>
/// <remarks>
///     All endpoints in this controller require JWT authentication.
///     The controller supports retrieving skills with and without related project data.
/// </remarks>
[ApiController]
[Route("api/developer/{developerId}/skills")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeveloperSkillsController : AuthorizedApiController
{
    private readonly IDeveloperSkillManager _developerSkillManager;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the DeveloperSkillsController.
    /// </summary>
    /// <param name="developerSkillManager">Service for managing developer skill operations</param>
    /// <param name="mapper">AutoMapper instance for DTO mappings</param>
    public DeveloperSkillsController(
        IDeveloperSkillManager developerSkillManager,
        IMapper mapper)
    {
        _developerSkillManager = developerSkillManager;
        _mapper = mapper;
    }

    /// <summary>
    ///     Retrieves all skills associated with a specific developer.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer whose skills to retrieve</param>
    /// <returns>API response containing a collection of the developer's skills</returns>
    /// <remarks>
    ///     This endpoint uses no-tracking queries for improved read performance.
    ///     Returns basic skill information without related entities.
    /// </remarks>
    /// <response code="200">Successfully retrieved the list of skills</response>
    /// <response code="401">User is not authenticated or token is invalid</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<DeveloperSkillResponse>>>> GetDeveloperSkills(
        string developerId)
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Retrieve skills using no-tracking query for better performance
        var developerSkills = await _developerSkillManager.GetAllDeveloperSkillsNoTrackingAsync(developerId, UserId);

        // Map domain entities to DTO responses
        var response = _mapper.Map<IEnumerable<DeveloperSkillResponse>>(developerSkills);

        return Ok(new ApiResponse<IEnumerable<DeveloperSkillResponse>>
        {
            Data = response,
            Message = "Successfully retrieved skills",
            Success = true
        });
    }

    /// <summary>
    ///     Retrieves all skills associated with a specific developer, including related project information.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer whose skills to retrieve</param>
    /// <returns>API response containing a collection of the developer's skills with project details</returns>
    /// <remarks>
    ///     This endpoint uses no-tracking queries for improved read performance.
    ///     Returns detailed skill information including associated project data.
    /// </remarks>
    /// <response code="200">Successfully retrieved the list of skills with project details</response>
    /// <response code="401">User is not authenticated or token is invalid</response>
    [HttpGet("with-projects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<DeveloperSkillDetailsResponse>>>>
        GetDeveloperSkillsWithProjects(string developerId)
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Retrieve skills with related project data using no-tracking query
        var developerSkills =
            await _developerSkillManager.GetAllDeveloperSkillsWithProjectNoTrackingAsync(developerId, UserId);

        // Map domain entities to detailed DTO responses
        var response = _mapper.Map<IEnumerable<DeveloperSkillDetailsResponse>>(developerSkills);

        return Ok(new ApiResponse<IEnumerable<DeveloperSkillDetailsResponse>>
        {
            Data = response,
            Message = "Successfully retrieved skills with projects",
            Success = true
        });
    }
}