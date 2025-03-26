using BL.Interfaces.Developers;
using BL.Interfaces.Experiences;
using DAL.Interfaces;
using Domain.Entities;
using Shared.DTOs.Requests.Experience;

namespace BL.Managers.Experiences;

/// <inheritdoc cref="IExperienceManager" />
public class ExperienceManager(
    IExperienceRepository experienceRepository,
    IDeveloperRepository developerRepository,
    IExperienceAccessor experienceAccessor,
    IDeveloperAccessor developerAccessor) : IExperienceManager
{
    /// <inheritdoc />
    public async Task DeleteExperienceAsync(string developerId, string experienceId, string externalUserId)
    {
        // Parse the experience ID to a GUID
        var guidId = ParseGuid(experienceId);

        // Retrieve the experience with its associated developer
        var experience = await experienceRepository.GetByIdWithDeveloperAsync(guidId);

        // Validate access to the experience
        await experienceAccessor.CheckAccessAsync(experience, developerId, externalUserId);

        // Delete the experience
        await experienceRepository.DeleteByIdAsync(guidId);
    }

    /// <inheritdoc />
    public async Task<Experience> GetExperienceAsync(string developerId, string experienceId, string externalUserId)
    {
        // Parse the experience ID to a GUID
        var guidId = ParseGuid(experienceId);

        // Retrieve the experience with its developer (no tracking)
        var experience = await experienceRepository.GetByIdWithDeveloperNoTrackingAsync(guidId);

        // Validate access and return the experience
        return await experienceAccessor.CheckAccessAsync(experience, developerId, externalUserId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Experience>> GetAllExperiencesAsync(string developerId, string externalUserId)
    {
        // Parse the developer ID to a GUID
        var guidId = ParseGuid(developerId);

        // Retrieve the developer (no tracking)
        var developer = await developerRepository.GetByIdNoTrackingAsync(guidId);

        // Validate access to the developer
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Retrieve all experiences for the developer
        return await experienceRepository.GetAllByDeveloperIdNoTrackingAsync(guidId);
    }

    /// <inheritdoc />
    public async Task<Experience> AddExperienceAsync(string developerId, CreateExperienceRequest dto,
        string externalUserId)
    {
        // Parse the developer ID to a GUID
        var guidId = ParseGuid(developerId);

        // Retrieve the developer
        var developer = await developerRepository.GetByIdAsync(guidId);

        // Validate access to the developer
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Create a new experience
        var experience = Experience.Create(
            dto.ExperienceTypeName,
            dto.StartDate,
            dto.EndDate,
            dto.LocationName,
            dto.Title,
            dto.Description,
            developer);

        // Add experience to developer and save changes
        developer.AddExperience(experience);
        await developerRepository.SaveChangesAsync();

        return experience;
    }

    /// <inheritdoc />
    public async Task UpdateExperienceAsync(string developerId, string experienceId, UpdateExperienceRequest dto,
        string externalUserId)
    {
        // Parse the experience ID to a GUID
        var guidId = ParseGuid(experienceId);

        // Retrieve the experience with its associated developer
        var experience = await experienceRepository.GetByIdWithDeveloperAsync(guidId);

        // Validate access to the experience
        await experienceAccessor.CheckAccessAsync(experience, developerId, externalUserId);

        // Update the experience details
        experience.Update(
            dto.ExperienceTypeName,
            dto.StartDate,
            dto.EndDate,
            dto.LocationName,
            dto.Title,
            dto.Description);

        // Save changes
        await experienceRepository.SaveChangesAsync();
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