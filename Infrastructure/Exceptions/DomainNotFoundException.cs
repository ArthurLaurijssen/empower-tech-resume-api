namespace Infrastructure.Exceptions;

public class BlobNotFoundException(string message) : Exception(message)
{
}