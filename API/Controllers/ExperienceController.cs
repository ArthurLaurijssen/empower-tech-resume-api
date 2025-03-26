using API.Responses;
using AutoMapper;
using BL.Interfaces.Experiences;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests.Experience;
using Shared.DTOs.Response.Experience;

namespace API.Controllers;

/// <summary>
///     Controller for managing developer experience entries.
///     Provides endpoints for CRUD operations on experience records.
/// </summary>
/// <remarks>
///     All endpoints require JWT authentication by default except for GET operations.
///     Administrative access is required for modification operations.
/// </remarks>
[ApiController]
[Route("api/developer/{developerId}/experience")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ExperienceController : AuthorizedApiController
{
    private readonly IExperienceManager _experienceManager;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the ExperienceController.
    /// </summary>
    /// <param name="experienceManager">Service for managing experience operations</param>
    /// <param name="mapper">AutoMapper instance for DTO mappings</param>
    public ExperienceController(IExperienceManager experienceManager, IMapper mapper)
    {
        _experienceManager = experienceManager;
        _mapper = mapper;
    }

    /// <summary>
    ///     Adds a new experience entry to a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="experienceDto">The experience details to be added</param>
    /// <returns>API response containing the newly created experience entry's ID</returns>
    /// <response code="200">Returns when experience is successfully added</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpPost]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<CreateExperienceResponse>>> AddExperience(
        string developerId,
        [FromBody] CreateExperienceRequest experienceDto)
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Create new experience entry
        var experience = await _experienceManager.AddExperienceAsync(
            developerId,
            experienceDto,
            UserId);

        return Ok(new ApiResponse<CreateExperienceResponse>
        {
            Data = new CreateExperienceResponse { ExperienceId = experience.Id },
            Message = "Successfully added experience",
            Success = true
        });
    }

    /// <summary>
    ///     Updates an existing experience entry in a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="experienceId">The unique identifier of the experience to update</param>
    /// <param name="experienceDto">The updated experience details</param>
    /// <returns>API response indicating update success</returns>
    /// <response code="200">Returns when experience is successfully updated</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpPut("{experienceId}")]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> UpdateExperience(
        string developerId,
        string experienceId,
        [FromBody] UpdateExperienceRequest experienceDto)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        await _experienceManager.UpdateExperienceAsync(
            developerId,
            experienceId,
            experienceDto,
            UserId);

        return Ok(new ApiResponse
        {
            Message = "Successfully updated experience",
            Success = true
        });
    }

    /// <summary>
    ///     Deletes an experience entry from a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="experienceId">The unique identifier of the experience to delete</param>
    /// <returns>API response indicating deletion success</returns>
    /// <response code="200">Returns when experience is successfully deleted</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpDelete("{experienceId}")]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> DeleteExperience(
        string developerId,
        string experienceId)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        await _experienceManager.DeleteExperienceAsync(
            developerId,
            experienceId,
            UserId);

        return Ok(new ApiResponse
        {
            Message = "Successfully deleted experience",
            Success = true
        });
    }

    /// <summary>
    ///     Retrieves a specific experience entry from a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="experienceId">The unique identifier of the experience to retrieve</param>
    /// <returns>API response containing the requested experience details</returns>
    /// <response code="200">Returns when experience is successfully retrieved</response>
    /// <response code="401">Returns when user is not authorized</response>
    [HttpGet("{experienceId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<ExperienceResponse>>> GetExperience(
        string developerId,
        string experienceId)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Retrieve the requested experience
        var experience = await _experienceManager.GetExperienceAsync(
            developerId,
            experienceId,
            UserId);

        // Map domain entity to DTO response
        var response = _mapper.Map<ExperienceResponse>(experience);

        return Ok(new ApiResponse<ExperienceResponse>
        {
            Data = response,
            Message = "Successfully retrieved experience",
            Success = true
        });
    }
}