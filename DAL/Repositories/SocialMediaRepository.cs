using DAL.Data;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class SocialMediaRepository(ResumeDbContext context):ISocialMediaRepository
{
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.SocialMediaLinks
            .AsNoTracking()
            .AnyAsync(d => d.Id == id);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await context.SocialMediaLinks
            .Where(d => d.Id == id)
            .ExecuteDeleteAsync();
    }
}