namespace Shared.DTOs.Response.Project;

public record ProjectResponse
{
    public Guid Id { get; init; }
    public string ImageUrl { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
}