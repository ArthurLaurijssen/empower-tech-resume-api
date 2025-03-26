using DAL.Data;
using DAL.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

/// <summary>
///     Repository implementation for Developers entity operations.
/// </summary>
public class DeveloperRepository(ResumeDbContext context) : IDeveloperRepository
{
    public async Task<Developer?> GetByIdAsync(Guid id)
    {
        return await context.Developers
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Developer?> GetByIdWithSocialMediaLinksAsync(Guid id)
    {
        return await context.Developers.Include(d => d.SocialMediaLinks)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Developer?> GetByIdNoTrackingAsync(Guid id)
    {
        return await context.Developers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Developer?> GetByIdWithDetailsAsync(Guid id)
    {
        return await context.Developers
            .Include(d => d.Experiences)
            .Include(d => d.SocialMediaLinks)
            .Include(d => d.DeveloperProficiencies)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Developer?> GetByIdWithDetailsNoTrackingAsync(Guid id)
    {
        return await context.Developers
            .Include(d => d.Experiences)
            .Include(d => d.SocialMediaLinks)
            .Include(d => d.DeveloperProficiencies)
            .ThenInclude(dp => dp.Projects)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Developer>> GetAllAsync()
    {
        return await context.Developers
            .ToListAsync();
    }

    public async Task<IEnumerable<Developer>> GetAllNoTrackingAsync()
    {
        return await context.Developers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Developer>> GetAllWithDetailsAsync()
    {
        return await context.Developers
            .Include(d => d.Experiences)
            .Include(d => d.SocialMediaLinks)
            .Include(d => d.DeveloperProficiencies)
            .ThenInclude(p => p.Projects)
            .ToListAsync();
    }

    public async Task<IEnumerable<Developer>> GetAllWithDetailsNoTrackingAsync()
    {
        return await context.Developers
            .AsNoTracking()
            .Include(d => d.Experiences)
            .Include(d => d.SocialMediaLinks)
            .Include(d => d.DeveloperProficiencies)
            .ThenInclude(p => p.Projects)
            .ToListAsync();
    }

    public async Task RegisterAsync(Developer developer)
    {
        await context.Developers.AddAsync(developer);
        await SaveChangesAsync();
    }

    public async Task RemoveAsync(Developer developer)
    {
        context.Developers.Remove(developer);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Developers
            .AsNoTracking()
            .AnyAsync(d => d.Id == id);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await context.Developers
            .Where(d => d.Id == id)
            .ExecuteDeleteAsync();
    }
}