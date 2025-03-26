using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
///     Represents a professional or educational experience entry for a developer.
///     This entity tracks details about work history, education, or other relevant experiences
///     including dates, location, and descriptions.
/// </summary>
public class Experience : BaseEntity
{
    /// <summary>
    ///     Initializes a new instance of the Experience class.
    ///     Required for Entity Framework Core.
    /// </summary>
    private Experience()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the Experience class with the specified parameters.
    ///     Performs validation on all input parameters before creating the instance.
    /// </summary>
    /// <param name="experienceType">Type of experience (e.g., work, education)</param>
    /// <param name="startDate">Date when the experience began</param>
    /// <param name="endDate">Optional date when the experience ended</param>
    /// <param name="locationName">Name of the location where the experience occurred</param>
    /// <param name="title">Title or position held</param>
    /// <param name="description">Detailed description of the experience</param>
    /// <param name="developer">Developer associated with this experience</param>
    /// <exception cref="DomainValidationException">Thrown when validation fails for any parameter</exception>
    private Experience(ExperienceType experienceType, DateTimeOffset startDate, DateTimeOffset? endDate,
        string locationName, string title, string description, Developer developer)
    {
        // Validate all input parameters
        ValidateDates(startDate, endDate);
        ValidateStrings(locationName, title, description);

        ExperienceType = experienceType;
        StartDate = startDate;
        EndDate = endDate;
        LocationName = locationName;
        Title = title;
        Description = description;
        Developer = developer;
    }

    /// <summary>
    ///     Gets the type of experience (e.g., work, education).
    /// </summary>
    public ExperienceType ExperienceType { get; private set; }

    /// <summary>
    ///     Gets the date when this experience began.
    /// </summary>
    public DateTimeOffset StartDate { get; private set; }

    /// <summary>
    ///     Gets the optional date when this experience ended. Null indicates current/ongoing experience.
    /// </summary>
    public DateTimeOffset? EndDate { get; private set; }

    /// <summary>
    ///     Gets the name of the location where this experience occurred (e.g., company name, school name).
    /// </summary>
    public string LocationName { get; private set; }

    /// <summary>
    ///     Gets the title or position held during this experience.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    ///     Gets the detailed description of this experience.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    ///     Gets the developer associated with this experience.
    /// </summary>
    public Developer Developer { get; private set; }

    /// <summary>
    ///     Creates a new Experience instance with the specified parameters.
    /// </summary>
    /// <param name="experienceTypeName">String name of the experience type (case-insensitive)</param>
    /// <param name="startDate">Date when the experience began</param>
    /// <param name="endDate">Optional date when the experience ended</param>
    /// <param name="locationName">Name of the location where the experience occurred</param>
    /// <param name="title">Title or position held</param>
    /// <param name="description">Detailed description of the experience</param>
    /// <param name="developer">Developer associated with this experience</param>
    /// <returns>A new instance of Experience</returns>
    /// <exception cref="DomainValidationException">Thrown when any validation fails or experience type is invalid</exception>
    public static Experience Create(string experienceTypeName, DateTimeOffset startDate, DateTimeOffset? endDate,
        string locationName, string title, string description, Developer developer)
    {
        // Parse and validate experience type from string
        if (!Enum.TryParse<ExperienceType>(experienceTypeName, true, out var experienceType))
            throw new DomainValidationException($"Invalid experience type: {experienceTypeName}");

        return new Experience(experienceType, startDate, endDate, locationName, title, description, developer);
    }

    /// <summary>
    ///     Validates the chronological order of start and end dates.
    /// </summary>
    /// <param name="startDate">Start date to validate</param>
    /// <param name="endDate">Optional end date to validate</param>
    /// <exception cref="DomainValidationException">Thrown when end date is earlier than or equal to start date</exception>
    private static void ValidateDates(DateTimeOffset startDate, DateTimeOffset? endDate)
    {
        if (endDate.HasValue && startDate >= endDate.Value)
            throw new DomainValidationException("Start date must be before end date");
    }

    /// <summary>
    ///     Validates that required string fields are not empty or whitespace.
    /// </summary>
    /// <param name="locationName">Location name to validate</param>
    /// <param name="title">Title to validate</param>
    /// <param name="description">Description to validate</param>
    /// <exception cref="DomainValidationException">Thrown when any required field is empty or whitespace</exception>
    private static void ValidateStrings(string locationName, string title, string description)
    {
        if (string.IsNullOrWhiteSpace(locationName))
            throw new DomainValidationException("Location name cannot be empty");
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException("Title cannot be empty");
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainValidationException("Description cannot be empty");
    }

    /// <summary>
    ///     Updates all fields of the experience with new values.
    /// </summary>
    /// <param name="experienceTypeName">New experience type name (case-insensitive)</param>
    /// <param name="startDate">New start date</param>
    /// <param name="endDate">New optional end date</param>
    /// <param name="locationName">New location name</param>
    /// <param name="title">New title</param>
    /// <param name="description">New description</param>
    /// <exception cref="DomainValidationException">Thrown when any validation fails or experience type is invalid</exception>
    public void Update(string experienceTypeName, DateTimeOffset startDate, DateTimeOffset? endDate,
        string locationName, string title, string description)
    {
        // Validate all new values
        ValidateDates(startDate, endDate);
        ValidateStrings(locationName, title, description);

        // Parse and validate new experience type
        if (!Enum.TryParse<ExperienceType>(experienceTypeName, true, out var experienceType))
            throw new DomainValidationException($"Invalid experience type: {experienceTypeName}");

        // Update all fields
        ExperienceType = experienceType;
        StartDate = startDate;
        EndDate = endDate;
        LocationName = locationName;
        Title = title;
        Description = description;
    }
}