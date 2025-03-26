namespace Domain.Exceptions;

/// <summary>
///     Exception thrown when a requested domain entity or value object cannot be found.
/// </summary>
public class DomainNotFoundException(string message) : Exception(message)
{
}