namespace Shared.DTOs.Response.Project;

public record CreateProjectResponse
{
    public Guid ProjectId { get; init; }
}