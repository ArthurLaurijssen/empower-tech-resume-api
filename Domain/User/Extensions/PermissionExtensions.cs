namespace Domain.User.Extensions;

public static class PermissionExtensions
{
    public static bool HasDeveloperAccess(this User user, string? developerId = null)
    {
        return user.HasPermission("Developers", developerId);
    }
}