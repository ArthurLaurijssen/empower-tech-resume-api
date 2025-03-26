using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Response.Developer;

namespace API.Mapping.Profiles;

/// <summary>
///     AutoMapper profile for mapping social media-related entities to their corresponding DTOs.
///     Handles mapping of social media link information and network types.
/// </summary>
public class SocialMediaMappingProfile : Profile
{
    /// <summary>
    ///     Initializes the mapping configurations for social media-related entities.
    /// </summary>
    /// <remarks>
    ///     Defines mappings for:
    ///     - Basic social media link information (SocialMediaLinkResponse)
    ///     - Converts network enum to string representation
    /// </remarks>
    public SocialMediaMappingProfile()
    {
        CreateMap<SocialMediaLink, SocialMediaLinkResponse>()
            // Map primary identifier
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            // Map social media URL
            .ForMember(dest => dest.SocialMediaUrl,
                opt => opt.MapFrom(src => src.SocialMediaUrl))
            // Map network type with enum conversion
            .ForMember(dest => dest.Network,
                opt => opt.MapFrom(src => src.Network.ToString()));
    }
}