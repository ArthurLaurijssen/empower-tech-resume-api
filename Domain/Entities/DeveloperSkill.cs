using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
///     Represents a developer's proficiency in a specific technology, tracking skill level and related projects.
///     This entity maintains the relationship between a developer, their skill level in a technology,
///     and the projects where they've applied this technology.
/// </summary>
public class DeveloperSkill : BaseEntity
{
    // Private backing field for collection
    private readonly List<Project> _projects;

    /// <summary>
    ///     Initializes a new instance of the DeveloperSkill class with an empty projects collection.
    /// </summary>
    private DeveloperSkill()
    {
        // Initialize the Projects collection to prevent null reference exceptions
        _projects = new List<Project>();
        Projects = _projects;
    }

    /// <summary>
    ///     Initializes a new instance of the DeveloperSkill class with specified parameters.
    /// </summary>
    /// <param name="technologyName">Name of the technology or skill</param>
    /// <param name="proficiencyLevel">Proficiency level between -1 and 100 with -1 for no rating</param>
    /// <param name="developer">Developer associated with this skill</param>
    /// <exception cref="DomainNumberOutOfRangeException">Thrown when proficiency level is not between 0 and 100</exception>
    private DeveloperSkill(string technologyName, float proficiencyLevel, Developer developer) : this()
    {
        // Validate proficiency level range
        if (proficiencyLevel < -1 || proficiencyLevel > 100)
            throw new DomainNumberOutOfRangeException("Proficiency must be between -1 and 100");

        TechnologyName = technologyName;
        ProficiencyLevel = proficiencyLevel;
        Developer = developer;
    }

    /// <summary>
    ///     Gets the name of the technology or skill.
    /// </summary>
    public string TechnologyName { get; private set; }

    /// <summary>
    ///     Gets the proficiency level (0-100) in this technology.
    /// </summary>
    public float ProficiencyLevel { get; private set; }

    /// <summary>
    ///     Gets the developer associated with this skill.
    /// </summary>
    public Developer Developer { get; private set; }

    /// <summary>
    ///     Gets the collection of projects where this skill has been applied.
    ///     This collection is managed by Entity Framework Core.
    /// </summary>
    public ICollection<Project> Projects { get; private set; }

    /// <summary>
    ///     Creates a new DeveloperSkill instance with the specified parameters.
    /// </summary>
    /// <param name="technologyName">Name of the technology or skill</param>
    /// <param name="proficiencyLevel">Proficiency level between 0 and 100</param>
    /// <param name="developer">Developer associated with this skill</param>
    /// <returns>A new instance of DeveloperSkill</returns>
    /// <exception cref="DomainValidationException">Thrown when technology name is empty</exception>
    /// <exception cref="DomainNumberOutOfRangeException">Thrown when proficiency level is not between 0 and 100</exception>
    public static DeveloperSkill Create(string technologyName, float proficiencyLevel, Developer developer)
    {
        // Validate technology name
        if (string.IsNullOrWhiteSpace(technologyName))
            throw new DomainValidationException("Technology name cannot be empty");

        return new DeveloperSkill(technologyName, proficiencyLevel, developer);
    }

    /// <summary>
    ///     Updates the proficiency level for this skill.
    /// </summary>
    /// <param name="newLevel">New proficiency level between 0 and 100</param>
    /// <exception cref="DomainValidationException">Thrown when new level is not between -1 and 100 with -1 for no rating</exception>
    public void UpdateLevel(float newLevel)
    {
        // Validate new proficiency level range
        if (newLevel is < -1 or > 100)
            throw new DomainValidationException("Proficiency must be between -1 and 100");

        ProficiencyLevel = newLevel;
    }

    /// <summary>
    ///     Updates the name of the technology for this skill.
    /// </summary>
    /// <param name="technologyName">New technology name</param>
    /// <exception cref="DomainValidationException">Thrown when technology name is empty</exception>
    public void UpdateName(string technologyName)
    {
        // Validate technology name
        if (string.IsNullOrWhiteSpace(technologyName))
            throw new DomainValidationException("Technology name cannot be empty");

        TechnologyName = technologyName;
    }

    /// <summary>
    ///     Adds a project to the collection of projects where this skill has been applied.
    /// </summary>
    /// <param name="project">Project to add</param>
    public void AddProject(Project project)
    {
        _projects.Add(project);
    }
}