using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Shared.DTOs.Response.Developer;

namespace API.Mapping.Profiles;

/// <summary>
///     AutoMapper profile for mapping developer-related entities to their corresponding DTOs.
///     Handles basic and detailed developer information mappings.
/// </summary>
public class DeveloperMappingProfile : Profile
{
    /// <summary>
    ///     Initializes the mapping configurations for developer-related entities.
    /// </summary>
    /// <remarks>
    ///     Defines mappings for:
    ///     - Basic developer information (DeveloperResponse)
    ///     - Detailed developer information (DeveloperDetailsResponse)
    ///     - Greeting value object
    ///     - Mission value object
    /// </remarks>
    public DeveloperMappingProfile()
    {
        // Configure basic developer response mapping
        CreateMap<Developer, DeveloperResponse>()
            // Map primary identifier
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            // Map image URL with null handling
            .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ImageUrl) ? null : src.ImageUrl))
            // Map greeting and mission properties
            .ForMember(dest => dest.Greeting,
                opt => opt.MapFrom(src => src.Greeting))
            .ForMember(dest => dest.Mission,
                opt => opt.MapFrom(src => src.Mission));

        // Configure detailed developer response mapping
        CreateMap<Developer, DeveloperDetailsResponse>()
            // Inherit basic developer mappings
            .IncludeBase<Developer, DeveloperResponse>()
            // Map and order experiences by start date
            .ForMember(dest => dest.Experiences,
                opt => opt.MapFrom(src => src.Experiences.OrderByDescending(e => e.StartDate)))
            // Map and order proficiencies by level
            .ForMember(dest => dest.DeveloperProficiencies,
                opt => opt.MapFrom(src => src.DeveloperProficiencies.OrderByDescending(p => p.ProficiencyLevel)))
            // Map social media links
            .ForMember(dest => dest.SocialMediaLinks,
                opt => opt.MapFrom(src => src.SocialMediaLinks));

        // Configure greeting value object mapping
        CreateMap<Greeting, GreetingResponse>()
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Message,
                opt => opt.MapFrom(src => src.Message));

        // Configure mission value object mapping
        CreateMap<Mission, MissionResponse>()
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description));
    }
}