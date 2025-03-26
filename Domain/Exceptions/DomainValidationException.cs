namespace Domain.Exceptions;

/// <summary>
///     Exception thrown when domain validation rules are violated.
/// </summary>
public class DomainValidationException(string message) : Exception(message)
{
}