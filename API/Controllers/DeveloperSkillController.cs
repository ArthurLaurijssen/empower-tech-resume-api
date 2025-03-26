using API.Responses;
using AutoMapper;
using BL.Interfaces.DeveloperSkills;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests.DeveloperSkill;
using Shared.DTOs.Response.DeveloperSkill;

namespace API.Controllers;

/// <summary>
///     Controller for managing developer skills.
///     Handles CRUD operations for skills associated with specific developers.
/// </summary>
/// <remarks>
///     All endpoints require JWT authentication by default except for GET operations.
///     Administrative access is required for modification operations.
/// </remarks>
[ApiController]
[Route("api/developer/{developerId}/skill")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeveloperSkillController : AuthorizedApiController
{
    private readonly IDeveloperSkillManager _developerSkillManager;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the DeveloperSkillController.
    /// </summary>
    /// <param name="developerSkillManager">Service for managing developer skill operations</param>
    /// <param name="mapper">AutoMapper instance for DTO mappings</param>
    public DeveloperSkillController(IDeveloperSkillManager developerSkillManager, IMapper mapper)
    {
        _developerSkillManager = developerSkillManager;
        _mapper = mapper;
    }

    /// <summary>
    ///     Adds a new skill to a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="developerSkillDto">The skill details to be added</param>
    /// <returns>API response containing the newly created skill's ID</returns>
    /// <response code="200">Returns when skill is successfully added</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpPost]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<CreateDeveloperSkillResponse>>> AddSkill(
        string developerId,
        [FromBody] CreateDeveloperSkillRequest developerSkillDto)
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Add the new skill to the developer's profile
        var developerSkill = await _developerSkillManager.AddDeveloperSkillAsync(
            developerId,
            developerSkillDto,
            UserId);

        return Ok(new ApiResponse<CreateDeveloperSkillResponse>
        {
            Data = new CreateDeveloperSkillResponse { SkillId = developerSkill.Id },
            Message = "Successfully added skill",
            Success = true
        });
    }

    /// <summary>
    ///     Updates an existing skill in a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="skillId">The unique identifier of the skill to update</param>
    /// <param name="developerSkillDto">The updated skill details</param>
    /// <returns>API response indicating update success</returns>
    /// <response code="200">Returns when skill is successfully updated</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpPut("{skillId}")]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> UpdateSkill(
        string developerId,
        string skillId,
        [FromBody] UpdateDeveloperSkillRequest developerSkillDto)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        await _developerSkillManager.UpdateDeveloperSkillAsync(
            developerId,
            skillId,
            developerSkillDto,
            UserId);

        return Ok(new ApiResponse
        {
            Message = "Successfully updated skill",
            Success = true
        });
    }

    /// <summary>
    ///     Deletes a skill from a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="skillId">The unique identifier of the skill to delete</param>
    /// <returns>API response indicating deletion success</returns>
    /// <response code="200">Returns when skill is successfully deleted</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpDelete("{skillId}")]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> DeleteSkill(
        string developerId,
        string skillId)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        await _developerSkillManager.DeleteDeveloperSkillAsync(
            developerId,
            skillId,
            UserId);

        return Ok(new ApiResponse
        {
            Message = "Successfully deleted skill",
            Success = true
        });
    }

    /// <summary>
    ///     Retrieves a specific skill from a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="skillId">The unique identifier of the skill to retrieve</param>
    /// <returns>API response containing the requested skill details</returns>
    /// <response code="200">Returns when skill is successfully retrieved</response>
    /// <response code="401">Returns when user is not authorized</response>
    [HttpGet("{skillId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<DeveloperSkillResponse>>> GetSkill(
        string developerId,
        string skillId)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Retrieve the requested skill
        var skill = await _developerSkillManager.GetDeveloperSkillAsync(
            developerId,
            skillId,
            UserId);

        // Map the domain entity to DTO response
        var response = _mapper.Map<DeveloperSkillResponse>(skill);

        return Ok(new ApiResponse<DeveloperSkillResponse>
        {
            Data = response,
            Message = "Successfully retrieved skill",
            Success = true
        });
    }
}