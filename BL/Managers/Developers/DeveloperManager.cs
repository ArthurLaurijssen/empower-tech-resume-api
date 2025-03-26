using BL.Interfaces.Developers;
using BL.Interfaces.Users;
using DAL.Interfaces;
using Domain.Entities;
using Infrastructure.Services.Interfaces;
using Shared.DTOs.Requests.Developer;

namespace BL.Managers.Developers;

/// <inheritdoc cref="IDeveloperManager" />
public class DeveloperManager(
    IDeveloperRepository developerRepository,
    IPermissionManager permissionManager,
    IDeveloperAccessor developerAccessor,
    IBlobService blobService) : IDeveloperManager
{
    /// <inheritdoc />
    public async Task<Developer> AddDefaultDeveloperWithPermissionToCreator(string createdByUserId)
    {
        // Create an empty developer profile
        var developer = Developer.CreateEmptyDeveloper(createdByUserId);

        // Register the new developer in the repository
        await developerRepository.RegisterAsync(developer);

        // Grant specific permissions to the creator
        await permissionManager.GiveUserSpecificPermissionAsync(createdByUserId, "Developers",
            developer.Id.ToString());

        return developer;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Developer>> GetAllBasicDevelopersAsync(string externalUserId)
    {
        // Retrieve all developers without full tracking
        var developers = await developerRepository.GetAllNoTrackingAsync();

        // Filter developers based on user access rights
        return await developerAccessor.FilterAccessibleDevelopers(developers, externalUserId);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Developer>> GetAllDevelopersWithDetailsAsync(string externalUserId)
    {
        // Retrieve all developers with full details without tracking
        var developers = await developerRepository.GetAllWithDetailsNoTrackingAsync();

        // Filter developers based on user access rights
        return await developerAccessor.FilterAccessibleDevelopers(developers, externalUserId);
    }

    /// <inheritdoc />
    public async Task DeleteDeveloper(string developerId, string externalUserId)
    {
        // Parse and validate the developer ID
        var guidId = ParseDeveloperId(developerId);

        // Check if the developer exists
        if (!await developerRepository.ExistsAsync(guidId))
            throw new KeyNotFoundException($"Developers with ID {developerId} not found.");

        // Retrieve the developer
        var developer = await developerRepository.GetByIdAsync(guidId);

        // Validate access rights
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Delete the developer from the repository
        await developerRepository.DeleteByIdAsync(guidId);

        // Remove associated permissions
        await permissionManager.RemoveSpecificPermissionFromAllUsersAsync("Developers", developerId);

        // Delete associated blob storage directory
        await blobService.DeleteDirectoryAsync("images", $"developers/{developerId}/", true);
    }

    /// <inheritdoc />
    public async Task<Developer> GetDeveloperByIdAsync(string developerId)
    {
        // Parse the developer ID
        var guidId = ParseDeveloperId(developerId);

        // Retrieve developer without full tracking
        var developer = await developerRepository.GetByIdNoTrackingAsync(guidId);

        if (developer is null)
        {
            throw new KeyNotFoundException($"Developers with ID {developerId} not found.");
        }

        return developer;
    }

    /// <inheritdoc />
    public async Task<Developer> GetDeveloperWithDetailsByIdAsync(string developerId)
    {
        // Parse the developer ID to a GUID
        var guidId = ParseDeveloperId(developerId);

        // Retrieve developer with full details without tracking
        var developer = await developerRepository.GetByIdWithDetailsNoTrackingAsync(guidId);
        
        if (developer is null)
        {
            throw new KeyNotFoundException($"Developers with ID {developerId} not found.");
        }

        return developer;
    }

    /// <inheritdoc />
    public async Task UpdateDeveloperAsync(string developerId, UpdateDeveloperRequest dto, string externalUserId)
    {
        // Parse the developer ID
        var guidId = ParseDeveloperId(developerId);

        // Retrieve the developer
        var developer = await developerRepository.GetByIdAsync(guidId);

        // Validate access rights
        await developerAccessor.CheckAccessAsync(developer, developerId, externalUserId);

        // Update developer profile
        developer.UpdateProfile(
            dto.Name,
            dto.Email,
            dto.GreetingTitle,
            dto.GreetingMessage,
            dto.MissionTitle,
            dto.MissionDescription,
            dto.ItExperienceStartDate,
            dto.WorkExperienceStartDate
        );

        // Update profile image
        var imagePath = $"developers/{developerId}/profile/";
        var files = await blobService.GetBlobUrlsAsync("images", imagePath);
        if (files.Any()) developer.UpdateImageUrl(files.First());

        // Save changes 
        await developerRepository.SaveChangesAsync();
    }

    /// <summary>
    ///     Parses a developer ID string into a valid GUID.
    /// </summary>
    /// <param name="developerId">The developer ID string to parse.</param>
    /// <returns>A valid GUID representing the developer ID.</returns>
    /// <exception cref="FormatException">Thrown when the developer ID is not a valid GUID.</exception>
    private static Guid ParseDeveloperId(string developerId)
    {
        if (!Guid.TryParse(developerId, out var guidId))
            throw new FormatException($"Invalid developer ID format: {developerId}");
        return guidId;
    }
}