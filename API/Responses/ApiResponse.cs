namespace API.Responses;

/// <summary>
///     Base class for API responses. Provides common properties for all API responses.
/// </summary>
public class ApiResponse
{
    /// <summary>
    ///     Gets or sets the message describing the result of the API operation.
    /// </summary>
    /// <remarks>
    ///     Provides context about the API operation outcome, such as success confirmations
    ///     or helpful error details.
    /// </remarks>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a value indicating whether the API operation was successful.
    /// </summary>
    /// <remarks>
    ///     True indicates successful completion of the requested operation.
    ///     False indicates the operation failed or encountered errors.
    /// </remarks>
    public bool Success { get; set; }
}

/// <summary>
///     Generic API response class that includes a data payload.
/// </summary>
/// <typeparam name="T">The type of data being returned in the response</typeparam>
/// <remarks>
///     Extends the base ApiResponse to include strongly-typed data in the response.
///     Used when an API endpoint needs to return data along with the status information.
/// </remarks>
public class ApiResponse<T> : ApiResponse
{
    /// <summary>
    ///     Gets or sets the data payload of the response.
    /// </summary>
    /// <remarks>
    ///     Contains the actual data returned by the API operation.
    ///     May be null if the operation was unsuccessful or no data was returned.
    /// </remarks>
    public T? Data { get; set; }
}