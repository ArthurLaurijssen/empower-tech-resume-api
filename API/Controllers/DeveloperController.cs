using API.Responses;
using AutoMapper;
using BL.Interfaces.Developers;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests.Developer;
using Shared.DTOs.Response.Developer;

namespace API.Controllers;

/// <summary>
///     Controller responsible for managing developer-related operations in the API.
///     Provides endpoints for creating, updating, deleting, and retrieving developer information.
/// </summary>
/// <remarks>
///     This controller requires JWT authentication by default and includes both admin-restricted
///     and public endpoints. It inherits from AuthorizedApiController to leverage common authorization functionality.
/// </remarks>
[ApiController]
[Route("api/developer")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeveloperController : AuthorizedApiController
{
    private readonly IDeveloperManager _developerManager;
    private readonly ILogger<DeveloperController> _logger;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the DeveloperController.
    /// </summary>
    /// <param name="developerManager">Service for managing developer operations</param>
    /// <param name="logger">Logger for recording controller activities</param>
    /// <param name="mapper">AutoMapper instance for DTO mappings</param>
    public DeveloperController(
        IDeveloperManager developerManager,
        ILogger<DeveloperController> logger,
        IMapper mapper)
    {
        _developerManager = developerManager;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    ///     Creates a new developer with default permissions assigned to the creator.
    /// </summary>
    /// <returns>API response containing the newly created developer's ID</returns>
    /// <response code="200">Returns when developer is successfully created</response>
    /// <response code="401">Returns when user is not authorized</response>
    [HttpPost("add-new-default")]
    [Authorize(Policy = "RequireAdminAccess")]
    public async Task<ActionResult<ApiResponse<CreateDeveloperResponse>>> AddDefaultDeveloper()
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Create developer with default permissions
        var developer = await _developerManager.AddDefaultDeveloperWithPermissionToCreator(UserId);

        // Log successful creation
        _logger.LogInformation(
            "Default developer added successfully. Developer ID: {DeveloperId}, Created by: {UserId}",
            developer.Id, UserId);

        return Ok(new ApiResponse<CreateDeveloperResponse>
        {
            Data = new CreateDeveloperResponse { DeveloperId = developer.Id },
            Message = "Default developer added successfully.",
            Success = true
        });
    }

    /// <summary>
    ///     Deletes a specific developer by their ID.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer to delete</param>
    /// <returns>API response indicating deletion success</returns>
    /// <response code="200">Returns when developer is successfully deleted</response>
    /// <response code="401">Returns when user is not authorized</response>
    [HttpDelete("{developerId}")]
    [Authorize(Policy = "RequireAdminAccess")]
    public async Task<ActionResult<ApiResponse>> DeleteDeveloper(string developerId)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        await _developerManager.DeleteDeveloper(developerId, UserId);

        return Ok(new ApiResponse
        {
            Message = "Developer deleted successfully.",
            Success = true
        });
    }

    /// <summary>
    ///     Updates a developer's information.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer to update</param>
    /// <param name="updateDeveloperRequest">The updated developer information</param>
    /// <returns>API response indicating update success</returns>
    /// <response code="200">Returns when developer is successfully updated</response>
    /// <response code="401">Returns when user is not authorized</response>
    [HttpPut("{developerId}")]
    [Authorize(Policy = "RequireAdminAccess")]
    public async Task<ActionResult<ApiResponse>> UpdateDeveloper(
        string developerId,
        [FromBody] UpdateDeveloperRequest updateDeveloperRequest)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        await _developerManager.UpdateDeveloperAsync(developerId, updateDeveloperRequest, UserId);

        return Ok(new ApiResponse
        {
            Message = "Developer updated successfully.",
            Success = true
        });
    }

    /// <summary>
    ///     Retrieves basic information about a specific developer.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer to retrieve</param>
    /// <returns>API response containing the developer's basic information</returns>
    /// <response code="200">Returns when developer is successfully retrieved</response>
    /// <response code="401">Returns when user is not authorized</response>
    [HttpGet("{developerId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<Developer>>> GetDeveloperById(string developerId)
    {

        var developer = await _developerManager.GetDeveloperByIdAsync(developerId);
        // Map the domain entity to DTO response
        var response = _mapper.Map<DeveloperResponse>(developer);

        return Ok(new ApiResponse<DeveloperResponse>
        {
            Data = response,
            Message = "Developer retrieved successfully.",
            Success = true
        });
    }

    /// <summary>
    ///     Retrieves detailed information about a specific developer, including additional details.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer to retrieve</param>
    /// <returns>API response containing the developer's detailed information</returns>
    /// <response code="200">Returns when developer details are successfully retrieved</response>
    /// <response code="401">Returns when user is not authorized</response>
    [HttpGet("{developerId}/with-details")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<Developer>>> GetDeveloperWithDetailsById(string developerId)
    {

        //Get the developer with detailed information
        var developer = await _developerManager.GetDeveloperWithDetailsByIdAsync(developerId);
        _logger.LogInformation(developer.DeveloperProficiencies.Count.ToString());
        foreach (var skill in developer.DeveloperProficiencies)
        {
            _logger.LogInformation(skill.TechnologyName);
            foreach (var project in skill.Projects)
            {
                _logger.LogInformation(project.Description);
                
            }
            
        }
        // Map the detailed domain entity to DTO response
        var response = _mapper.Map<DeveloperDetailsResponse>(developer);

        return Ok(new ApiResponse<DeveloperDetailsResponse>
        {
            Data = response,
            Message = "Developer details retrieved successfully.",
            Success = true
        });
    }
}