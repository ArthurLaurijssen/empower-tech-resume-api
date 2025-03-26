using DAL.Data;
using DAL.Interfaces;
using Domain.User;
using Domain.User.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository(ResumeDbContext context) : IUserRepository
{
    public async Task<User?> GetByExternalIdAsync(string externalId)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.ExternalId == externalId);
    }

    public async Task<User?> GetByExternalIdIncludingPermissionsAsync(string externalId)
    {
        return await context.Users
            .Include(u => u.Permissions)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.ExternalId == externalId);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task RegisterAsync(User user)
    {
        await context.Users.AddAsync(user);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsersWithSpecificPermissionAsync(string resource, string resourceId)
    {
        return await context.Users
            .Include(u => u.Permissions)
            .Where(u => u.Permissions.Any(p =>
                p.Resource == resource &&
                p.ResourceId == resourceId &&
                p.Scope == PermissionScope.Specific))
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}