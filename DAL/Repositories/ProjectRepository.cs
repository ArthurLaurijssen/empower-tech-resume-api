using DAL.Data;
using DAL.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ProjectRepository(ResumeDbContext context) : IProjectRepository
{
    public async Task<Project?> GetByIdWithDetailsAsync(Guid id)
    {
        return await context.Projects
            .Include(p => p.DeveloperSkills)
            .ThenInclude(ds => ds.Developer)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project?> GetByIdWithDetailsNoTrackingAsync(Guid id)
    {
        return await context.Projects
            .Include(p => p.DeveloperSkills)
            .ThenInclude(ds => ds.Developer)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetAllByDeveloperSkillIdNoTrackingAsync(Guid skillId)
    {
        return await context.Projects
            .Where(p => p.DeveloperSkills.Any(ds => ds.Id == skillId))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task RegisterAsync(Project project)
    {
        await context.Projects.AddAsync(project);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Projects
            .AsNoTracking()
            .AnyAsync(p => p.Id == id);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        await context.Projects
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }
}