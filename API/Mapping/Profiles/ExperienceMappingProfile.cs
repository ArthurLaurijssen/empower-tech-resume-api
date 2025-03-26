using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Response.Experience;

namespace API.Mapping.Profiles;

/// <summary>
///     AutoMapper profile for mapping experience-related entities to their corresponding DTOs.
///     Handles mapping of developer experience information.
/// </summary>
public class ExperienceMappingProfile : Profile
{
    /// <summary>
    ///     Initializes the mapping configurations for experience-related entities.
    /// </summary>
    /// <remarks>
    ///     Defines mappings for:
    ///     - Basic experience information (ExperienceResponse) including:
    ///     * Unique identifier
    ///     * Experience type classification
    ///     * Date range (start and end dates)
    ///     * Location information
    ///     * Role/position details
    /// </remarks>
    public ExperienceMappingProfile()
    {
        CreateMap<Experience, ExperienceResponse>()
            // Map primary identifier
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            // Map experience classification
            .ForMember(dest => dest.ExperienceType,
                opt => opt.MapFrom(src => src.ExperienceType))
            // Map temporal information
            .ForMember(dest => dest.StartDate,
                opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate,
                opt => opt.MapFrom(src => src.EndDate))
            // Map location details
            .ForMember(dest => dest.LocationName,
                opt => opt.MapFrom(src => src.LocationName))
            // Map role/position information
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description));
    }
}