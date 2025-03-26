namespace Infrastructure.Exceptions;

public class BlobOperationException(string message) : Exception(message)
{
}