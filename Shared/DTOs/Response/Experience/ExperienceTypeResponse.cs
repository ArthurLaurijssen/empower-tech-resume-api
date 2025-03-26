namespace Shared.DTOs.Response.Experience;

public record ExperienceTypeResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
}