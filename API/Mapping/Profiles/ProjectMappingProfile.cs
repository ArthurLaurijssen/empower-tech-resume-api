using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Response.Project;

namespace API.Mapping.Profiles;

/// <summary>
///     AutoMapper profile for mapping project-related entities to their corresponding DTOs.
///     Handles mapping of project information including media assets.
/// </summary>
public class ProjectMappingProfile : Profile
{
    /// <summary>
    ///     Initializes the mapping configurations for project-related entities.
    /// </summary>
    /// <remarks>
    ///     Defines mappings for:
    ///     - Basic project information (ProjectResponse)
    ///     - Handles null image URLs appropriately
    ///     - Maps descriptive content and metadata
    /// </remarks>
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectResponse>()
            // Map primary identifier
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            // Map image URL with null handling
            .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ImageUrl) ? null : src.ImageUrl))
            // Map project details
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description));
    }
}