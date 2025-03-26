namespace Domain.Exceptions;

/// <summary>
///     Exception thrown when a numeric value in the domain model falls outside its valid range.
/// </summary>
public class DomainNumberOutOfRangeException(string message) : Exception(message)
{
}