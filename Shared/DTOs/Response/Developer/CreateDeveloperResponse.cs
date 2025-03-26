namespace Shared.DTOs.Response.Developer;

public record CreateDeveloperResponse
{
    public Guid DeveloperId { get; init; }
}