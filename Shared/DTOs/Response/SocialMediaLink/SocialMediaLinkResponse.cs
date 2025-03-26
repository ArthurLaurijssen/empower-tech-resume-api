using System.Text.Json.Serialization;
using Domain.Enums;

namespace Shared.DTOs.Response.SocialMediaLink;

public record SocialMediaLinkResponse
{
    public Guid Id { get; init; }
    public string SocialMediaUrl { get; init; } = default!;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SocialMediaNetwork Network { get; init; }
}