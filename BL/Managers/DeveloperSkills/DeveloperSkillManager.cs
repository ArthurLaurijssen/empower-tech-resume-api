using BL.Interfaces.Developers;
using BL.Interfaces.DeveloperSkills;
using DAL.Interfaces;
using Domain.Entities;
using Shared.DTOs.Requests.DeveloperSkill;

namespace BL.Managers.DeveloperSkills;

/// <inheritdoc cref="IDeveloperSkillManager" />
public class DeveloperSkillManager(
    IDeveloperRepository developerRepository,
    IDeveloperSkillAccessor skillAccessor,
    IDeveloperAccessor developerAccessor,
    IDeveloperSkillRepository developerSkillRepository) : IDeveloperSkillManager
{
    /// <inheritdoc />
    public async Task<DeveloperSkill> AddDeveloperSkillAsync(string developerId, CreateDeveloperSkillRequest dto,
        string externalUserId)
    {
        // Parse the developer ID to a GUID
        var guidId = ParseGuid(developerId);

        // Retrieve the developer and validate access
        var developer = await developerRepository.GetByIdAsync(guidId);
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Create a new developer skill and add it to the developer's profile
        var skill = DeveloperSkill.Create(dto.Name, dto.ProficiencyLevel, developer);
        developer.AddDeveloperProficiency(skill);

        // Save changes to persist the new skill
        await developerRepository.SaveChangesAsync();

        return skill;
    }

    /// <inheritdoc />
    public async Task UpdateDeveloperSkillAsync(string developerId, string skillId, UpdateDeveloperSkillRequest dto,
        string externalUserId)
    {
        // Parse the skill ID to a GUID
        var skillGuidId = ParseGuid(skillId);

        // Retrieve the skill with its associated developer
        var skill = await developerSkillRepository.GetByIdWithDeveloperAsync(skillGuidId);

        // Validate access to the skill
        await skillAccessor.CheckAccessAsync(skill, developerId, externalUserId);

        // Update skill details
        skill.UpdateLevel(dto.ProficiencyLevel);
        skill.UpdateName(dto.Name);

        // Save changes
        await developerSkillRepository.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteDeveloperSkillAsync(string developerId, string skillId, string externalUserId)
    {
        // Parse the skill ID to a GUID
        var skillGuidId = ParseGuid(skillId);

        // Retrieve the skill with its associated developer
        var skill = await developerSkillRepository.GetByIdWithDeveloperAsync(skillGuidId);

        // Validate access to the skill
        await skillAccessor.CheckAccessAsync(skill, developerId, externalUserId);

        // Delete the skill
        await developerSkillRepository.DeleteByIdAsync(skillGuidId);
    }

    /// <inheritdoc />
    public async Task<DeveloperSkill> GetDeveloperSkillAsync(string developerId, string skillId, string externalUserId)
    {
        // Parse the skill ID to a GUID
        var skillGuidId = ParseGuid(skillId);

        // Retrieve the skill with its developer (no tracking)
        var skill = await developerSkillRepository.GetByIdWithDeveloperNoTrackingAsync(skillGuidId);

        // Validate access and return the skill
        return await skillAccessor.CheckAccessAsync(skill, developerId, externalUserId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<DeveloperSkill>> GetAllDeveloperSkillsNoTrackingAsync(string developerId,
        string externalUserId)
    {
        // Parse the developer ID to a GUID
        var guidId = ParseGuid(developerId);

        // Retrieve the developer (no tracking)
        var developer = await developerRepository.GetByIdNoTrackingAsync(guidId);

        // Validate access to the developer
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Retrieve all skills for the developer
        return await developerSkillRepository.GetAllByDeveloperIdNoTrackingAsync(guidId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<DeveloperSkill>> GetAllDeveloperSkillsWithProjectNoTrackingAsync(string developerId,
        string externalUserId)
    {
        // Parse the developer ID to a GUID
        var guidId = ParseGuid(developerId);

        // Retrieve the developer (no tracking)
        var developer = await developerRepository.GetByIdNoTrackingAsync(guidId);

        // Validate access to the developer
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Retrieve all skills with associated projects
        return await developerSkillRepository.GetAllByDeveloperIdWithProjectsNoTrackingAsync(guidId);
    }

    /// <inheritdoc />
    public async Task<DeveloperSkill> GetDeveloperSkillWithProjectsAsync(string developerId, string skillId,
        string externalUserId)
    {
        // Parse the skill ID to a GUID
        var skillGuidId = ParseGuid(skillId);

        // Retrieve the skill with developer and projects (no tracking)
        var skill = await developerSkillRepository.GetByIdWithDeveloperAndProjectsNoTrackingAsync(skillGuidId);

        // Validate access and return the skill
        return await skillAccessor.CheckAccessAsync(skill, developerId, externalUserId);
    }

    /// <summary>
    ///     Parses a string identifier into a GUID.
    /// </summary>
    /// <param name="id">The string identifier to parse.</param>
    /// <returns>A valid GUID representation of the identifier.</returns>
    /// <exception cref="FormatException">Thrown when the identifier cannot be parsed to a GUID.</exception>
    private static Guid ParseGuid(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new FormatException($"Invalid ID format: {id}");
        return guid;
    }
}