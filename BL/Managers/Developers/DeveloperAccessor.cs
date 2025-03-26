using BL.Exceptions;
using BL.Interfaces.Developers;
using BL.Interfaces.Users;
using Domain.Entities;
using Domain.User;
using Domain.User.Extensions;

namespace BL.Managers.Developers;

/// <inheritdoc cref="IDeveloperAccessor" />
public class DeveloperAccessor(IUserManager userManager) : IDeveloperAccessor
{
    /// <inheritdoc />
    public async Task<Developer> CheckAccessAsync(Developer? developer, string developerId, string externalUserId)
    {
        if (developer is null)
            throw new KeyNotFoundException($"Developers with ID {developerId} not found.");

        var user = await userManager.GetOrCreateUserIncludingPermissionsAsync(externalUserId);
        if (!CanAccessDeveloper(user, developer))
            throw new DeveloperAccessDeniedException(externalUserId, developerId);

        return developer;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Developer>> FilterAccessibleDevelopers(IEnumerable<Developer> developers,
        string externalUserId)
    {
        var user = await userManager.GetOrCreateUserIncludingPermissionsAsync(externalUserId);
        return user.HasDeveloperAccess()
            ? developers
            : developers.Where(dev => CanAccessDeveloper(user, dev));
    }

    /// <summary>
    ///     Determines if a user has access to a specific developer.
    /// </summary>
    /// <param name="user">The user attempting to access the developer.</param>
    /// <param name="developer">The developer being accessed.</param>
    /// <returns>True if the user has access, otherwise false.</returns>
    /// <remarks>
    ///     Access is granted if:
    ///     - The user has global developer access
    ///     - The user has specific access to the developer
    ///     - The user created the developer
    /// </remarks>
    private static bool CanAccessDeveloper(User user, Developer developer)
    {
        return user.HasDeveloperAccess() ||
               user.HasDeveloperAccess(developer.Id.ToString()) ||
               developer.CreatedById == user.ExternalId;
    }
}