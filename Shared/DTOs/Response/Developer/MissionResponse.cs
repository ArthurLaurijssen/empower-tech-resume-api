namespace Shared.DTOs.Response.Developer;

public record MissionResponse
{
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
}