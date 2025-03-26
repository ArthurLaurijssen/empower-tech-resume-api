using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
///     Represents a project entity that showcases developer skills and achievements.
///     This entity maintains project details and its relationships with developer skills.
/// </summary>
public class Project : BaseEntity
{
    // Private backing field for collection
    private readonly List<DeveloperSkill> _developerSkills;

    /// <summary>
    ///     Initializes a new instance of the Project class with an empty skills collection.
    ///     Required for Entity Framework Core.
    /// </summary>
    private Project()
    {
        // Initialize the DeveloperSkills collection to prevent null reference exceptions
        _developerSkills = new List<DeveloperSkill>();
        DeveloperSkills = _developerSkills;
    }

    /// <summary>
    ///     Initializes a new instance of the Project class with specified parameters.
    /// </summary>
    /// <param name="imageUrl">URL to the project's image or thumbnail</param>
    /// <param name="title">Title of the project</param>
    /// <param name="description">Detailed description of the project</param>
    private Project(string imageUrl, string title, string description) : this()
    {
        ImageUrl = imageUrl;
        Title = title;
        Description = description;
    }

    /// <summary>
    ///     Gets the URL to the project's image or thumbnail.
    /// </summary>
    public string ImageUrl { get; private set; }

    /// <summary>
    ///     Gets the title of the project.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    ///     Gets the detailed description of the project.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    ///     Gets the collection of developer skills utilized in this project.
    ///     This represents the many-to-many relationship between projects and developer skills.
    ///     This collection is managed by Entity Framework Core.
    /// </summary>
    public ICollection<DeveloperSkill> DeveloperSkills { get; private set; }

    /// <summary>
    ///     Creates a new Project instance with the specified parameters.
    /// </summary>
    /// <param name="imageUrl">URL to the project's image or thumbnail</param>
    /// <param name="title">Title of the project</param>
    /// <param name="description">Detailed description of the project</param>
    /// <returns>A new instance of Project</returns>
    /// <exception cref="DomainValidationException">Thrown when any required field is empty or whitespace</exception>
    public static Project Create(string imageUrl, string title, string description)
    {
        // Validate all required fields
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainValidationException("Image URL cannot be empty");
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException("Title cannot be empty");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainValidationException("Description cannot be empty");

        return new Project(imageUrl, title, description);
    }

    /// <summary>
    ///     Updates the project's title and description.
    /// </summary>
    /// <param name="title">New title for the project</param>
    /// <param name="description">New description for the project</param>
    /// <exception cref="DomainValidationException">Thrown when title or description is empty or whitespace</exception>
    public void Update(string title, string description)
    {
        // Validate updated fields
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException("Title cannot be empty");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainValidationException("Description cannot be empty");

        Title = title;
        Description = description;
    }

    /// <summary>
    ///     Updates the project's image URL.
    /// </summary>
    /// <param name="imageUrl">New image URL for the project</param>
    /// <exception cref="DomainValidationException">Thrown when image URL is empty or whitespace</exception>
    public void UpdateImageUrl(string imageUrl)
    {
        // Validate image URL
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainValidationException("Image URL cannot be empty");
        ImageUrl = imageUrl;
    }

    /// <summary>
    ///     Adds a developer skill to the project.
    /// </summary>
    /// <param name="skill">The developer skill to add to the project</param>
    public void AddDeveloperSkill(DeveloperSkill skill)
    {
        _developerSkills.Add(skill);
    }
}