using DAL.Data;
using DAL.Interfaces;
using Domain.User.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class PermissionRepository(ResumeDbContext context) : IPermissionRepository
{
    public async Task DeleteAllAsync(string resourceName, string resourceIdentifier)
    {
        await context.Permissions
            .Where(p =>
                p.Resource == resourceName &&
                p.ResourceId == resourceIdentifier &&
                p.Scope == PermissionScope.Specific).ExecuteDeleteAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}