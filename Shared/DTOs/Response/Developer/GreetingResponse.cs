namespace Shared.DTOs.Response.Developer;

public record GreetingResponse
{
    public string Title { get; init; } = default!;
    public string Message { get; init; } = default!;
}