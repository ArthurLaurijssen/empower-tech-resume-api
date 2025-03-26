using System.Text.Json.Serialization;

namespace API.Responses;

/// <summary>
///     Represents a structured error response returned to the client when an exception occurs.
/// </summary>
/// <remarks>
///     This record provides a consistent error format that includes:
///     - HTTP status code for protocol-level error indication
///     - Human-readable error message for user display
///     - Machine-readable error code for programmatic handling
///     - Detailed error information for debugging
///     - Optional resource identifier for context
/// </remarks>
public record ErrorResponse
{
   /// <summary>
   ///     The HTTP status code associated with the error.
   /// </summary>
   /// <remarks>
   ///     Common values include:
   ///     - 400 (Bad Request)
   ///     - 401 (Unauthorized)
   ///     - 403 (Forbidden)
   ///     - 404 (Not Found)
   ///     - 500 (Internal Server Error)
   /// </remarks>
   [JsonPropertyName("statusCode")]
    public int StatusCode { get; init; }

   /// <summary>
   ///     A short, human-readable description of the error.
   /// </summary>
   /// <remarks>
   ///     Intended for display to end users. Examples:
   ///     - "Not Found"
   ///     - "Unauthorized"
   ///     - "Invalid Input"
   ///     Should be concise and clear while avoiding technical jargon.
   /// </remarks>
   [JsonPropertyName("error")]
    public string Error { get; init; } = string.Empty;

   /// <summary>
   ///     A machine-readable error code for programmatic error handling.
   /// </summary>
   /// <remarks>
   ///     Uses the ErrorCode enum to ensure consistent error classification.
   ///     Suitable for client-side logic or error message localization.
   /// </remarks>
   [JsonPropertyName("code")]
    public ErrorCode Code { get; init; }

   /// <summary>
   ///     Detailed information about the error condition.
   /// </summary>
   /// <remarks>
   ///     May include:
   ///     - Validation error messages
   ///     - Stack traces (in development)
   ///     - Troubleshooting hints
   ///     - Technical details for developers
   /// </remarks>
   [JsonPropertyName("details")]
    public string Details { get; init; } = string.Empty;

   /// <summary>
   ///     Optional identifier for the resource involved in the error.
   /// </summary>
   /// <remarks>
   ///     Examples:
   ///     - Developer ID
   ///     - Project ID
   ///     - Document reference
   ///     May be null if no specific resource is involved.
   /// </remarks>
   [JsonPropertyName("id")]
    public string? Id { get; init; }
}