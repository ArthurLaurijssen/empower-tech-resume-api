namespace Domain.ValueObjects;

/// <summary>
///     Represents an immutable greeting with a title and message.
/// </summary>
public record Greeting
{
    /// <summary>
    ///     Constructor for EF Core
    /// </summary>
    private Greeting()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the Greeting record.
    /// </summary>
    /// <param name="title">The title of the greeting.</param>
    /// <param name="message">The message content of the greeting.</param>
    private Greeting(string title, string message)
    {
        Title = title;
        Message = message;
    }

    /// <summary>
    ///     Gets the title of the greeting.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    ///     Gets the message content of the greeting.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    ///     Creates a new Greeting instance with validation.
    /// </summary>
    /// <param name="title">The title of the greeting.</param>
    /// <param name="message">The message content of the greeting.</param>
    /// <returns>A new validated Greeting instance.</returns>
    /// <exception cref="ArgumentException">Thrown when title or message is null, empty, or whitespace.</exception>
    public static Greeting Create(string title, string message)
    {
        // Validate that title is not null, empty, or whitespace
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Een greeting moet een titel hebben");

        // Validate that message is not null, empty, or whitespace
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Een greeting moet een bericht hebben");

        //Return new Greeting instance
        return new Greeting(title, message);
    }

    /// <summary>
    ///     Creates a new Greeting instance with default values.
    ///     <returns>A new Greeting instance with default values.</returns>
    /// </summary>
    public static Greeting CreateDefaultGreeting()
    {
        return new Greeting("Welcome!", "I'm a software developer with passion for technology.");
    }
}