using System.Text.Json.Serialization;
using Domain.Enums;

namespace Shared.DTOs.Response.Experience;

public record ExperienceResponse
{
    public Guid Id { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExperienceType ExperienceType { get; init; } = default!;
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset? EndDate { get; init; }
    public string LocationName { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
}