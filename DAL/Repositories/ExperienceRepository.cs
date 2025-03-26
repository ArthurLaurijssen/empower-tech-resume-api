using DAL.Data;
using DAL.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ExperienceRepository(ResumeDbContext context) : IExperienceRepository
{
    public async Task<Experience?> GetByIdWithDeveloperAsync(Guid id)
    {
        return await context.Experiences
            .Include(e => e.Developer)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Experience?> GetByIdWithDeveloperNoTrackingAsync(Guid id)
    {
        return await context.Experiences
            .Include(e => e.Developer)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Experience>> GetAllByDeveloperIdNoTrackingAsync(Guid developerId)
    {
        return await context.Experiences
            .Where(e => e.Developer.Id == developerId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task RegisterAsync(Experience experience)
    {
        await context.Experiences.AddAsync(experience);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Experiences.AsNoTracking().AnyAsync(e => e.Id == id);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await context.Experiences.Where(e => e.Id == id).ExecuteDeleteAsync();
    }
}