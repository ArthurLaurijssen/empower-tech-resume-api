using BL.Interfaces.DeveloperSkills;
using BL.Interfaces.Projects;
using Domain.Entities;

namespace BL.Managers.Projects;

/// <inheritdoc cref="IProjectAccessor" />
public class ProjectAccessor(IDeveloperSkillAccessor skillAccessor) : IProjectAccessor
{
    /// <inheritdoc />
    public async Task<Project> CheckAccessAsync(Project? project, string externalUserId)
    {
        // Verify that the project and its associated developer exist
        if (project?.DeveloperSkills.FirstOrDefault()?.Developer is null)
            throw new KeyNotFoundException("Project or related developer not found.");

        // Get the first associated skill
        var skill = project.DeveloperSkills.First();

        // Validate access through the developer skill accessor
        await skillAccessor.CheckAccessAsync(skill, skill.Developer.Id.ToString(), externalUserId);

        return project;
    }
}