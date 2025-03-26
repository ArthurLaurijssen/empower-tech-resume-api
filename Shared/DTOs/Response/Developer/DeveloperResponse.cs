using Shared.DTOs.Response.DeveloperSkill;
using Shared.DTOs.Response.Experience;

namespace Shared.DTOs.Response.Developer;

public record DeveloperResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string? ImageUrl { get; init; }
    public GreetingResponse Greeting { get; init; } = default!;
    public MissionResponse Mission { get; init; } = default!;
    public DateTimeOffset ItExperienceStartDate { get; init; }
    public DateTimeOffset WorkExperienceStartDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}

public record DeveloperDetailsResponse : DeveloperResponse
{
    public IReadOnlyCollection<ExperienceResponse> Experiences { get; init; } = Array.Empty<ExperienceResponse>();

    public IReadOnlyCollection<SocialMediaLinkResponse> SocialMediaLinks { get; init; } =
        Array.Empty<SocialMediaLinkResponse>();

    public IReadOnlyCollection<DeveloperSkillDetailsResponse> DeveloperProficiencies { get; init; } =
        Array.Empty<DeveloperSkillDetailsResponse>();
}