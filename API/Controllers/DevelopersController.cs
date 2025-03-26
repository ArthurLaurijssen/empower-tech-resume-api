using API.Responses;
using AutoMapper;
using BL.Interfaces.Developers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Response.Developer;

namespace API.Controllers;

/// <summary>
///     Controller for managing operations related to retrieving collections of developers.
///     Provides endpoints for fetching both basic and detailed developer information.
/// </summary>
/// <remarks>
///     All endpoints in this controller require JWT authentication and inherit base authorization functionality.
///     The controller differentiates between basic and detailed developer information retrieval.
/// </remarks>
[ApiController]
[Route("api/developers")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DevelopersController : AuthorizedApiController
{
    private readonly IDeveloperManager _developerManager;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the DevelopersController.
    /// </summary>
    /// <param name="developerManager">Service for managing developer operations</param>
    /// <param name="mapper">AutoMapper instance for DTO mappings</param>
    public DevelopersController(IDeveloperManager developerManager, IMapper mapper)
    {
        _developerManager = developerManager;
        _mapper = mapper;
    }

    /// <summary>
    ///     Retrieves all developers with basic information.
    /// </summary>
    /// <returns>An API response containing a collection of developers with basic details</returns>
    /// <remarks>
    ///     This endpoint returns a simplified view of all developers, including only essential information.
    ///     The response is mapped to DeveloperResponse DTOs to ensure data transfer efficiency.
    /// </remarks>
    /// <response code="200">Successfully retrieved the list of developers</response>
    /// <response code="401">User is not authenticated or token is invalid</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<DeveloperResponse>>>> GetDevelopers()
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Retrieve all developers with basic information
        var developers = await _developerManager.GetAllBasicDevelopersAsync(UserId);

        // Map domain entities to DTO responses
        var response = _mapper.Map<IEnumerable<DeveloperResponse>>(developers);

        return Ok(new ApiResponse<IEnumerable<DeveloperResponse>>
        {
            Data = response,
            Message = "Developers retrieved successfully.",
            Success = true
        });
    }

    /// <summary>
    ///     Retrieves all developers with their complete information and related data.
    /// </summary>
    /// <returns>An API response containing a collection of developers with comprehensive details</returns>
    /// <remarks>
    ///     This endpoint provides an expanded view of all developers, including additional related data
    ///     such as associated projects, skills, and other detailed information.
    ///     The response is mapped to DeveloperDetailsResponse DTOs to include all related entities.
    /// </remarks>
    /// <response code="200">Successfully retrieved the list of developers with details</response>
    /// <response code="401">User is not authenticated or token is invalid</response>
    [HttpGet("with-details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<DeveloperDetailsResponse>>>> GetDevelopersWithDetails()
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Retrieve all developers with detailed information
        var developers = await _developerManager.GetAllDevelopersWithDetailsAsync(UserId);

        // Map domain entities to detailed DTO responses
        var response = _mapper.Map<IEnumerable<DeveloperDetailsResponse>>(developers);

        return Ok(new ApiResponse<IEnumerable<DeveloperDetailsResponse>>
        {
            Data = response,
            Message = "Developers with details retrieved successfully.",
            Success = true
        });
    }
}