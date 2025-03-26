using API.Responses;
using BL.Interfaces.Projects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests.Project;
using Shared.DTOs.Response.Project;

namespace API.Controllers;

/// <summary>
///     Controller for managing projects associated with developer skills.
///     Provides endpoints for CRUD operations on project records.
/// </summary>
/// <remarks>
///     All endpoints require JWT authentication and administrative access.
///     Projects are nested resources under developer skills.
///     Route pattern: api/developer/{developerId}/skill/{developerSkillId}/project
/// </remarks>
[ApiController]
[Route("api/developer/{developerId}/skill/{developerSkillId}/project")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProjectController : AuthorizedApiController
{
    private readonly IProjectManager _projectManager;

    /// <summary>
    ///     Initializes a new instance of the ProjectController.
    /// </summary>
    /// <param name="projectManager">Service for managing project operations</param>
    public ProjectController(IProjectManager projectManager)
    {
        _projectManager = projectManager;
    }

    /// <summary>
    ///     Adds a new project to a developer's skill.
    /// </summary>
    /// <param name="developerSkillId">The unique identifier of the developer skill</param>
    /// <param name="projectDto">The project details to be added</param>
    /// <returns>API response containing the newly created project's ID</returns>
    /// <response code="200">Returns when project is successfully added</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpPost]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<CreateProjectRequest>>> AddProject(
        string developerSkillId,
        [FromBody] CreateProjectRequest projectDto)
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Add new project to the specified developer skill
        var project = await _projectManager.AddProjectToDeveloperSkillAsync(
            developerSkillId,
            projectDto,
            UserId);

        return Ok(new ApiResponse<CreateProjectResponse>
        {
            Data = new CreateProjectResponse { ProjectId = project.Id },
            Message = "Successfully added project",
            Success = true
        });
    }

    /// <summary>
    ///     Updates an existing project.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project to update</param>
    /// <param name="projectDto">The updated project details</param>
    /// <returns>API response indicating update success</returns>
    /// <response code="200">Returns when project is successfully updated</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpPut("{projectId}")]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> UpdateProject(
        string projectId,
        [FromBody] UpdateProjectRequest projectDto)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        await _projectManager.UpdateProjectAsync(
            projectId,
            projectDto,
            UserId);

        return Ok(new ApiResponse
        {
            Message = "Successfully updated project",
            Success = true
        });
    }

    /// <summary>
    ///     Deletes a project.
    /// </summary>
    /// <param name="projectId">The unique identifier of the project to delete</param>
    /// <returns>API response indicating deletion success</returns>
    /// <response code="200">Returns when project is successfully deleted</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpDelete("{projectId}")]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> DeleteProject(string projectId)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        await _projectManager.DeleteProjectAsync(
            projectId,
            UserId);

        return Ok(new ApiResponse
        {
            Message = "Successfully deleted project",
            Success = true
        });
    }
}