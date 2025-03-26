using BL.Interfaces.Users;
using DAL.Interfaces;
using Domain.User;

namespace BL.Managers.Users;

/// <inheritdoc cref="IUserManager" />
public class UserManager(IUserRepository userRepository) : IUserManager
{
    /// <inheritdoc />
    public async Task<User> GetOrCreateUserAsync(string externalId)
    {
        // Attempt to retrieve user by external ID
        var user = await userRepository.GetByExternalIdAsync(externalId);

        // Create new user if not found
        if (user == null)
        {
            user = User.Create(externalId);
            await userRepository.RegisterAsync(user);
        }

        return user;
    }

    /// <inheritdoc />
    public async Task<User> GetOrCreateUserIncludingPermissionsAsync(string externalId)
    {
        // Attempt to retrieve user by external ID with permissions
        var user = await userRepository.GetByExternalIdIncludingPermissionsAsync(externalId);

        // Create new user if not found
        if (user == null)
        {
            user = User.Create(externalId);
            await userRepository.RegisterAsync(user);
        }

        return user;
    }
}