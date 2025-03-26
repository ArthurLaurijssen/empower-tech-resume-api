using BL.Interfaces.DeveloperSkills;
using BL.Interfaces.Projects;
using DAL.Interfaces;
using Domain.Entities;
using Infrastructure.Services.Interfaces;
using Shared.DTOs.Requests.Project;

namespace BL.Managers.Projects;

/// <inheritdoc cref="IProjectManager" />
public class ProjectManager(
    IProjectRepository projectRepository,
    IDeveloperSkillRepository skillRepository,
    IProjectAccessor projectAccessor,
    IDeveloperSkillAccessor skillAccessor,
    IBlobService blobService) : IProjectManager
{
    /// <inheritdoc />
    public async Task<Project> AddProjectToDeveloperSkillAsync(string skillId, CreateProjectRequest dto,
        string externalUserId)
    {
        // Parse the skill ID to a GUID
        var guidId = ParseGuid(skillId);

        // Retrieve the skill with its associated developer
        var skill = await skillRepository.GetByIdWithDeveloperAsync(guidId);

        // Validate access to the skill
        await skillAccessor.CheckAccessAsync(skill, skill.Developer.Id.ToString(), externalUserId);

        // Create a new project with a default image
        var project = Project.Create("/default.jpeg", dto.Title, dto.Description);

        // Add project to the skill
        skill.AddProject(project);
        await skillRepository.SaveChangesAsync();

        // Define temporary and destination directories for project image
        var tempDirectory = $"developers/{skill.Developer.Id}/skills/{skill.Id}/projects/temp/";
        var destinationDirectory = $"developers/{skill.Developer.Id}/skills/{skill.Id}/projects/{project.Id}";

        // Move temporary project image and update project image URL if available
        var imageUrl = await blobService.MoveTemporaryProjectImageAsync(
            "images",
            tempDirectory,
            destinationDirectory);

        if (imageUrl != null)
        {
            project.UpdateImageUrl(imageUrl);
            await skillRepository.SaveChangesAsync();
        }

        return project;
    }

    /// <inheritdoc />
    public async Task UpdateProjectAsync(string projectId, UpdateProjectRequest dto, string externalUserId)
    {
        // Parse the project ID to a GUID
        var guidId = ParseGuid(projectId);

        // Retrieve the project with its details
        var project = await projectRepository.GetByIdWithDetailsAsync(guidId);

        // Validate access to the project
        await projectAccessor.CheckAccessAsync(project, externalUserId);

        // Update project details
        project.Update(dto.Title, dto.Description);

        // Construct image path for the project
        var imagePath =
            $"developers/{project.DeveloperSkills.First().Developer.Id}/skills/{project.DeveloperSkills.First().Id}/projects/{project.Id}/";

        // Retrieve project image URLs
        var files = await blobService.GetBlobUrlsAsync("images", imagePath);

        // Update project image URL
        if (files.Any())
            project.UpdateImageUrl(files.First());
        else
            project.UpdateImageUrl("/default.jpeg");

        // Save changes
        await projectRepository.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteProjectAsync(string projectId, string externalUserId)
    {
        // Parse the project ID to a GUID
        var guidId = ParseGuid(projectId);

        // Retrieve the project with its details
        var project = await projectRepository.GetByIdWithDetailsAsync(guidId);

        // Validate access to the project
        await projectAccessor.CheckAccessAsync(project, externalUserId);

        // Delete the project
        await projectRepository.DeleteByIdAsync(guidId);
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