using BL.Interfaces.Users;
using DAL.Interfaces;
using Domain.User;

namespace BL.Managers.Users;

/// <inheritdoc cref="IPermissionManager" />
public class PermissionManager(
    IUserRepository userRepository,
    IUserManager userManager,
    IPermissionRepository permissionRepository) : IPermissionManager
{
    /// <inheritdoc />
    public async Task GiveUserAllPermissionAsync(string externalUserId, string resourceName)
    {
        // Retrieve or create the user
        var user = await userManager.GetOrCreateUserAsync(externalUserId);

        // Create and add an all-permission entry for the specified resource
        user.AddPermission(Permission.CreateAllPermission(resourceName));

        // Update the user in the repository
        await userRepository.UpdateAsync(user);
    }

    /// <inheritdoc />
    public async Task GiveUserSpecificPermissionAsync(string externalUserId, string resourceName,
        string resourceIdentifier)
    {
        // Retrieve or create the user
        var user = await userManager.GetOrCreateUserAsync(externalUserId);

        // Create and add a specific permission entry for the resource
        user.AddPermission(Permission.CreateSpecificPermission(resourceName, resourceIdentifier));

        // Update the user in the repository
        await userRepository.UpdateAsync(user);
    }

    /// <inheritdoc />
    public async Task RemoveSpecificPermissionFromAllUsersAsync(string resourceName, string resourceIdentifier)
    {
        // Delete all permissions for the specified resource and identifier
        await permissionRepository.DeleteAllAsync(resourceName, resourceIdentifier);
    }
}