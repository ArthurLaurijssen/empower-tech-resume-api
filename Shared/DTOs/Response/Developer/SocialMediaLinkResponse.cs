using Domain.Enums;

namespace Shared.DTOs.Response.Developer;

public record SocialMediaLinkResponse
{
    public Guid Id { get; init; }
    public string SocialMediaUrl { get; init; } = default!;
    public SocialMediaNetwork Network { get; init; }
}