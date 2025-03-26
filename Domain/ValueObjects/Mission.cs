namespace Domain.ValueObjects;

/// <summary>
///     Represents an immutable mission value object with a title and description.
/// </summary>
public record Mission
{
    /// <summary>
    ///     Constructor for EF Core
    /// </summary>
    private Mission()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the Mission record.
    /// </summary>
    /// <param name="title">The title of the mission.</param>
    /// <param name="description">The description of the mission.</param>
    private Mission(string title, string description)
    {
        Title = title;
        Description = description;
    }

    /// <summary>
    ///     Gets the title of the mission.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    ///     Gets the description of the mission.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    ///     Creates a new Mission instance with validation.
    /// </summary>
    /// <param name="title">The title of the mission.</param>
    /// <param name="description">The description of the mission.</param>
    /// <returns>A new validated Mission instance.</returns>
    /// <exception cref="ArgumentException">Thrown when title or description is null, empty, or whitespace.</exception>
    public static Mission Create(string title, string description)
    {
        // Validate that title is not null, empty, or whitespace
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Een missie moet een titel hebben");

        // Validate that description is not null, empty, or whitespace
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Een missie moet een beschrijving hebben");

        // Return a new Mission instance
        return new Mission(title, description);
    }

    /// <summary>
    ///     Creates a new Mission instance with default values.
    ///     <returns>A new Mission instance with default values.</returns>
    /// </summary>
    public static Mission CreateDefaultMission()
    {
        return new Mission("My Mission", "To create efficient and scalable software solutions.");
    }
}