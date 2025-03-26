using Shared.DTOs.Response.Project;

namespace Shared.DTOs.Response.DeveloperSkill;

public record DeveloperSkillResponse
{
    public Guid Id { get; init; }
    public string TechnologyName { get; init; } = default!;
    public float ProficiencyLevel { get; init; }
    
}

public record DeveloperSkillDetailsResponse : DeveloperSkillResponse
{
    public IReadOnlyCollection<ProjectResponse> Projects { get; init; } = Array.Empty<ProjectResponse>();
}