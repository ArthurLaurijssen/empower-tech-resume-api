using DAL.Data;
using DAL.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class DeveloperSkillRepository(ResumeDbContext context) : IDeveloperSkillRepository
{
    public async Task<DeveloperSkill?> GetByIdWithDeveloperAsync(Guid id)
    {
        return await context.DeveloperSkills
            .Include(ds => ds.Developer)
            .FirstOrDefaultAsync(s => s.Id == id);
    }


    public async Task RegisterAsync(DeveloperSkill developerSkill)
    {
        await context.DeveloperSkills.AddAsync(developerSkill);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.DeveloperSkills
            .AsNoTracking()
            .AnyAsync(s => s.Id == id);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await context.DeveloperSkills
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();
    }


    public async Task<DeveloperSkill?> GetByIdWithDeveloperNoTrackingAsync(Guid id)
    {
        return await context.DeveloperSkills
            .Include(ds => ds.Developer)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<DeveloperSkill?> GetByIdWithDeveloperAndProjectsNoTrackingAsync(Guid id)
    {
        return await context.DeveloperSkills
            .Include(ds => ds.Developer)
            .Include(ds => ds.Projects)
            .AsNoTracking()
            .FirstOrDefaultAsync(ds => ds.Id == id);
    }

    public async Task<IEnumerable<DeveloperSkill>> GetAllByDeveloperIdNoTrackingAsync(Guid developerId)
    {
        return await context.DeveloperSkills
            .Where(ds => ds.Developer.Id == developerId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<DeveloperSkill>> GetAllByDeveloperIdWithProjectsNoTrackingAsync(Guid developerId)
    {
        return await context.DeveloperSkills
            .Where(ds => ds.Developer.Id == developerId)
            .Include(ds => ds.Projects)
            .AsNoTracking()
            .ToListAsync();
    }
}