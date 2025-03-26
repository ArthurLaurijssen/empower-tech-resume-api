using BL.Exceptions;
using BL.Interfaces.Developers;
using BL.Interfaces.DeveloperSkills;
using Domain.Entities;

namespace BL.Managers.DeveloperSkills;

/// <inheritdoc cref="IDeveloperSkillAccessor" />
public class DeveloperSkillAccessor(IDeveloperAccessor developerAccessor) : IDeveloperSkillAccessor
{
    /// <inheritdoc />
    public async Task<DeveloperSkill> CheckAccessAsync(DeveloperSkill? skill, string developerId, string externalUserId)
    {
        // Verify that the skill and its associated developer exist
        if (skill?.Developer is null)
            throw new KeyNotFoundException("Developers skill not found.");

        // Ensure the skill belongs to the specified developer
        if (skill.Developer.Id.ToString() != developerId)
            throw new DeveloperAccessDeniedException(externalUserId, developerId);

        // Validate developer access using the developer accessor
        await developerAccessor.CheckAccessAsync(skill.Developer, developerId, externalUserId);

        return skill;
    }
}