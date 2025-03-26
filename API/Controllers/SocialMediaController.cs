using API.Responses;
using AutoMapper;
using BL.Interfaces.SocialMedia;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests.SocialMediaLink;
using Shared.DTOs.Response.SocialMediaLink;
using SocialMediaLinkResponse = Shared.DTOs.Response.Developer.SocialMediaLinkResponse;

namespace API.Controllers;

/// <summary>
///     Controller for managing developer social media links.
///     Provides endpoints for CRUD operations on social media connections.
/// </summary>
/// <remarks>
///     All endpoints require JWT authentication by default except for GET operations.
///     Administrative access is required for modification operations.
///     Each developer can have one link per social media network.
/// </remarks>
[ApiController]
[Route("api/developer/{developerId}/social-media")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SocialMediaController : AuthorizedApiController
{
    private readonly IMapper _mapper;
    private readonly ISocialMediaManager _socialMediaManager;

    /// <summary>
    ///     Initializes a new instance of the SocialMediaController.
    /// </summary>
    /// <param name="socialMediaManager">Service for managing social media operations</param>
    /// <param name="mapper">AutoMapper instance for DTO mappings</param>
    public SocialMediaController(ISocialMediaManager socialMediaManager, IMapper mapper)
    {
        _socialMediaManager = socialMediaManager;
        _mapper = mapper;
    }

    /// <summary>
    ///     Adds a new social media link to a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="createSocialMediaLinkRequest">The social media link details to be added</param>
    /// <returns>API response containing the newly created social media link's ID</returns>
    /// <response code="200">Returns when social media link is successfully added</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpPost]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<CreateSocialMediaLinkResponse>>> AddSocialMediaLink(
        string developerId,
        [FromBody] CreateSocialMediaLinkRequest createSocialMediaLinkRequest)
    {
        // Verify user authentication
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Add new social media link
        var socialMediaLink = await _socialMediaManager.AddSocialMediaLinkAsync(
            developerId,
            createSocialMediaLinkRequest,
            UserId);

        return Ok(new ApiResponse<CreateSocialMediaLinkResponse>
        {
            Data = new CreateSocialMediaLinkResponse { SocialMediaLinkId = socialMediaLink.Id },
            Message = "Successfully added social media link",
            Success = true
        });
    }

    /// <summary>
    ///     Deletes a social media link from a developer's profile.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <param name="network">The social media network to remove</param>
    /// <returns>API response indicating deletion success</returns>
    /// <response code="200">Returns when social media link is successfully deleted</response>
    /// <response code="401">Returns when user is not authorized</response>
    /// <response code="403">Returns when user lacks administrative access</response>
    [HttpDelete]
    [Authorize(Policy = "RequireAdminAccess")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> DeleteSocialMediaLink(
        string developerId,
        [FromQuery] SocialMediaNetwork network)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        await _socialMediaManager.DeleteSocialMediaLinkAsync(
            developerId,
            network,
            UserId);

        return Ok(new ApiResponse
        {
            Message = $"Successfully deleted {network} social media link",
            Success = true
        });
    }

    /// <summary>
    ///     Retrieves all social media links for a specific developer.
    /// </summary>
    /// <param name="developerId">The unique identifier of the developer</param>
    /// <returns>API response containing a collection of the developer's social media links</returns>
    /// <remarks>
    ///     Returns all social media links associated with the developer's profile.
    ///     This endpoint is accessible anonymously.
    /// </remarks>
    /// <response code="200">Returns when social media links are successfully retrieved</response>
    /// <response code="401">Returns when user is not authorized</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<SocialMediaLinkResponse>>>> GetAllSocialMediaLinks(
        string developerId)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return Unauthorized();

        // Retrieve all social media links for the developer
        var links = await _socialMediaManager.GetAllSocialMediaLinksAsync(
            developerId,
            UserId);

        // Map domain entities to DTO responses
        var response = _mapper.Map<IEnumerable<SocialMediaLinkResponse>>(links);

        return Ok(new ApiResponse<IEnumerable<SocialMediaLinkResponse>>
        {
            Data = response,
            Message = "Successfully retrieved social media links",
            Success = true
        });
    }
}