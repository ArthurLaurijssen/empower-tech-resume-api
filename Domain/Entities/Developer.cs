using System.Net.Mail;
using Domain.Common;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
///     Represents a developer entity within the system, implementing the aggregate root pattern.
///     This class encapsulates developer-related information including personal details, skills, experiences,
///     and social media presence.
/// </summary>
public class Developer : BaseEntity, IAggregateRoot
{
    // Private backing fields for collections
    private readonly List<DeveloperSkill> _developerProficiencies;
    private readonly List<Experience> _experiences;
    private readonly List<SocialMediaLink> _socialMediaLinks;

    /// <summary>
    ///     Initializes a new instance of the Developer class with default empty collections.
    /// </summary>
    private Developer()
    {
        // Initialize collections to prevent null reference exceptions
        _developerProficiencies = new List<DeveloperSkill>();
        _experiences = new List<Experience>();
        _socialMediaLinks = new List<SocialMediaLink>();

        // Initialize public properties to the same instances
        DeveloperProficiencies = _developerProficiencies;
        Experiences = _experiences;
        SocialMediaLinks = _socialMediaLinks;
    }

    /// <summary>
    ///     Initializes a new instance of the Developer class with specified parameters.
    /// </summary>
    /// <param name="name">The developer's full name</param>
    /// <param name="email">The developer's email address</param>
    /// <param name="greeting">The developer's personalized greeting</param>
    /// <param name="mission">The developer's professional mission statement</param>
    /// <param name="itExperienceStartDate">Date when IT experience began</param>
    /// <param name="workExperienceStartDate">Date when general work experience began</param>
    /// <param name="imageUrl">URL to the developer's profile image</param>
    /// <param name="createdById">Identifier of the user who created this profile</param>
    private Developer(
        string name,
        string email,
        Greeting greeting,
        Mission mission,
        DateTimeOffset itExperienceStartDate,
        DateTimeOffset workExperienceStartDate,
        string imageUrl,
        string createdById) : this()
    {
        Name = name;
        Email = email;
        Greeting = greeting;
        Mission = mission;
        ItExperienceStartDate = itExperienceStartDate;
        WorkExperienceStartDate = workExperienceStartDate;
        ImageUrl = imageUrl;
        CreatedById = createdById;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Gets the developer's full name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     Gets the URL to the developer's profile image.
    /// </summary>
    public string ImageUrl { get; private set; }

    /// <summary>
    ///     Gets the developer's email address.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    ///     Gets the developer's personalized greeting.
    /// </summary>
    public Greeting Greeting { get; private set; }

    /// <summary>
    ///     Gets the developer's professional mission statement.
    /// </summary>
    public Mission Mission { get; private set; }

    /// <summary>
    ///     Gets the date when the developer started their IT career.
    /// </summary>
    public DateTimeOffset ItExperienceStartDate { get; private set; }

    /// <summary>
    ///     Gets the date when the developer started their professional career.
    /// </summary>
    public DateTimeOffset WorkExperienceStartDate { get; private set; }

    /// <summary>
    ///     Gets the identifier of the user who created this profile.
    /// </summary>
    public string CreatedById { get; private set; }

    /// <summary>
    ///     Gets the UTC datetime when this profile was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    ///     Gets the collection of developer's professional experiences.
    ///     This collection is managed by Entity Framework Core.
    /// </summary>
    public ICollection<Experience> Experiences { get; private set; }

    /// <summary>
    ///     Gets the collection of developer's social media links.
    ///     This collection is managed by Entity Framework Core.
    /// </summary>
    public ICollection<SocialMediaLink> SocialMediaLinks { get; private set; }

    /// <summary>
    ///     Gets the collection of developer's technical proficiencies.
    ///     This collection is managed by Entity Framework Core.
    /// </summary>
    public ICollection<DeveloperSkill> DeveloperProficiencies { get; private set; }

    /// <summary>
    ///     Creates a new Developer instance with the specified parameters.
    /// </summary>
    /// <param name="name">The developer's full name</param>
    /// <param name="email">The developer's email address</param>
    /// <param name="greeting">The developer's personalized greeting</param>
    /// <param name="mission">The developer's professional mission statement</param>
    /// <param name="itExperienceStartDate">Date when IT experience began</param>
    /// <param name="workExperienceStartDate">Date when general work experience began</param>
    /// <param name="imageUrl">URL to the developer's profile image</param>
    /// <param name="createdById">Identifier of the user who created this profile</param>
    /// <returns>A new instance of Developer</returns>
    /// <exception cref="DomainValidationException">Thrown when validation fails for required fields</exception>
    public static Developer Create(
        string name,
        string email,
        Greeting greeting,
        Mission mission,
        DateTimeOffset itExperienceStartDate,
        DateTimeOffset workExperienceStartDate,
        string imageUrl,
        string createdById)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainValidationException("Name cannot be empty");
        if (!IsValidEmail(email))
            throw new DomainValidationException("Invalid email format");
        if (string.IsNullOrWhiteSpace(createdById))
            throw new DomainValidationException("Creator ID cannot be empty");

        return new Developer(
            name,
            email,
            greeting,
            mission,
            itExperienceStartDate,
            workExperienceStartDate,
            imageUrl,
            createdById);
    }

    /// <summary>
    ///     Creates a new Developer instance with default values.
    /// </summary>
    /// <param name="createdById">Identifier of the user who created this profile</param>
    /// <returns>A new instance of Developer with default values</returns>
    /// <exception cref="DomainValidationException">Thrown when creator ID is empty</exception>
    public static Developer CreateEmptyDeveloper(string createdById)
    {
        if (string.IsNullOrWhiteSpace(createdById))
            throw new DomainValidationException("Creator ID cannot be empty");

        return new Developer(
            "Default Developer",
            "default@example.com",
            Greeting.CreateDefaultGreeting(),
            Mission.CreateDefaultMission(),
            DateTime.UtcNow.AddYears(-5),
            DateTime.UtcNow.AddYears(-7),
            "",
            createdById);
    }

    /// <summary>
    ///     Updates the developer's profile information.
    /// </summary>
    /// <param name="name">New name</param>
    /// <param name="email">New email</param>
    /// <param name="greetingTitle">New greeting title</param>
    /// <param name="greetingMessage">New greeting message</param>
    /// <param name="missionTitle">New mission title</param>
    /// <param name="missionDescription">New mission description</param>
    /// <param name="itExperienceStartDate">New IT experience start date</param>
    /// <param name="workExperienceStartDate">New work experience start date</param>
    /// <exception cref="DomainValidationException">Thrown when validation fails for required fields</exception>
    public void UpdateProfile(
        string name,
        string email,
        string greetingTitle,
        string greetingMessage,
        string missionTitle,
        string missionDescription,
        DateTime itExperienceStartDate,
        DateTime workExperienceStartDate)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainValidationException("Name cannot be empty");
        if (!IsValidEmail(email))
            throw new DomainValidationException("Invalid email format");

        // Update profile properties
        Name = name;
        Email = email;
        Greeting = Greeting.Create(greetingTitle, greetingMessage);
        Mission = Mission.Create(missionTitle, missionDescription);
        ItExperienceStartDate = itExperienceStartDate;
        WorkExperienceStartDate = workExperienceStartDate;
    }

    /// <summary>
    ///     Updates the developer's profile image URL.
    /// </summary>
    /// <param name="imageUrl">New image URL</param>
    /// <exception cref="DomainValidationException">Thrown when image URL is empty</exception>
    public void UpdateImageUrl(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainValidationException("Image URL cannot be empty");
        ImageUrl = imageUrl;
    }

    /// <summary>
    ///     Adds a new experience to the developer's profile.
    /// </summary>
    /// <param name="experience">The experience to add</param>
    public void AddExperience(Experience experience)
    {
        _experiences.Add(experience);
    }

    /// <summary>
    ///     Adds a new social media link to the developer's profile.
    /// </summary>
    /// <param name="link">The social media link to add</param>
    public void AddSocialMediaLink(SocialMediaLink link)
    {
        _socialMediaLinks.Add(link);
    }

    /// <summary>
    ///     Adds a new technical skill to the developer's profile.
    /// </summary>
    /// <param name="skill">The technical skill to add</param>
    public void AddDeveloperProficiency(DeveloperSkill skill)
    {
        _developerProficiencies.Add(skill);
    }

    /// <summary>
    ///     Updates the proficiency level for a specific technology.
    /// </summary>
    /// <param name="technologyName">Name of the technology to update</param>
    /// <param name="newLevel">New proficiency level (0-100)</param>
    /// <exception cref="DomainNumberOutOfRangeException">Thrown when proficiency level is not between 0 and 100</exception>
    /// <exception cref="DomainNotFoundException">Thrown when technology is not found in proficiencies</exception>
    public void UpdateProficiency(string technologyName, float newLevel)
    {
        // Validate proficiency level range
        if (newLevel < 0 || newLevel > 100)
            throw new DomainNumberOutOfRangeException("Proficiency level must be between 0 and 100");

        // Find and update the proficiency
        var proficiency = _developerProficiencies.FirstOrDefault(p => p.TechnologyName == technologyName)
                          ?? throw new DomainNotFoundException($"No proficiency found for {technologyName}");

        proficiency.UpdateLevel(newLevel);
    }

    /// <summary>
    ///     Validates an email address format.
    /// </summary>
    /// <param name="email">Email address to validate</param>
    /// <returns>True if email is valid, false otherwise</returns>
    private static bool IsValidEmail(string email)
    {
        try
        {
            // Attempt to create a MailAddress instance to validate email format
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}