namespace BL.Exceptions;

public class DeveloperAccessDeniedException(string userId, string developerId)
    : UnauthorizedAccessException($"Users {userId} does not have permission to access developer {developerId}")
{
}