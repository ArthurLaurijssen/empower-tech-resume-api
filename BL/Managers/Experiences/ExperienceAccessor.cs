using BL.Exceptions;
using BL.Interfaces.Developers;
using BL.Interfaces.Experiences;
using Domain.Entities;

namespace BL.Managers.Experiences;

/// <inheritdoc cref="IExperienceAccessor"/>
public class ExperienceAccessor(IDeveloperAccessor developerAccessor) : IExperienceAccessor
{
    /// <inheritdoc/>
    public async Task<Experience> CheckAccessAsync(Experience? experience, string developerId, string externalUserId)
    {
        // Verify that the experience and its associated developer exist
        if (experience?.Developer is null)
            throw new KeyNotFoundException("Experience or related developer not found.");

        // Ensure the experience belongs to the specified developer
        if (experience.Developer.Id.ToString() != developerId)
            throw new DeveloperAccessDeniedException(externalUserId, developerId);

        // Validate developer access using the developer accessor
        await developerAccessor.CheckAccessAsync(experience.Developer, developerId, externalUserId);
       
        return experience;
    }
}