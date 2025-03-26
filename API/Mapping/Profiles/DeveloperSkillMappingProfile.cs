using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Response.DeveloperSkill;
using Shared.DTOs.Response.Project;

namespace API.Mapping.Profiles;
/// <summary>
///     AutoMapper profile for mapping developer skill-related entities to their corresponding DTOs.
///     Handles both basic and detailed skill information mappings.
/// </summary>
public class DeveloperSkillMappingProfile : Profile
{
    /// <summary>
    ///     Initializes the mapping configurations for developer skill-related entities.
    /// </summary>
    /// <remarks>
    ///     Defines mappings for:
    ///     - Basic developer skill information (DeveloperSkillResponse)
    ///     - Detailed developer skill information with projects (DeveloperSkillDetailsResponse)
    ///     Projects are ordered alphabetically by title in the detailed response.
    /// </remarks>
    public DeveloperSkillMappingProfile()
    {
        // Note: Project to ProjectResponse mapping is defined in ProjectMappingProfile
        // We don't need to redefine it here

        // Configure basic developer skill response mapping
        CreateMap<DeveloperSkill, DeveloperSkillResponse>()
            // Map primary identifier
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            // Map technology name
            .ForMember(dest => dest.TechnologyName,
                opt => opt.MapFrom(src => src.TechnologyName))
            // Map proficiency level
            .ForMember(dest => dest.ProficiencyLevel,
                opt => opt.MapFrom(src => src.ProficiencyLevel));

        // Configure detailed developer skill response mapping
        CreateMap<DeveloperSkill, DeveloperSkillDetailsResponse>()
            // Inherit basic skill mappings
            .IncludeBase<DeveloperSkill, DeveloperSkillResponse>()
            // Map and order projects alphabetically by title
            .ForMember(dest => dest.Projects,
                opt => opt.MapFrom(src => src.Projects.OrderBy(p => p.Title)));
    }
}