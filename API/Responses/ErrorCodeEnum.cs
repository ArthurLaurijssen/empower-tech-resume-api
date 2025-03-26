using System.Text.Json.Serialization;

namespace API.Responses;

/// <summary>
///     Enumerates the various error codes that can be returned by the exception handling middleware.
///     These codes help categorize errors for easier handling on the client side.
/// </summary>
/// <remarks>
///     Using an enum for error codes provides a controlled vocabulary for error types,
///     ensuring consistent interpretation across client and server.
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorCode
{
   /// <summary>
   ///     The provided token could not be validated (e.g., invalid signature, expired token).
   /// </summary>
   TOKEN_VALIDATION_FAILED,

   /// <summary>
   ///     The requested ID did not conform to the expected format (e.g., invalid GUID).
   /// </summary>
   INVALID_ID_FORMAT,

   /// <summary>
   ///     The request was made by an unauthorized user who lacks the necessary permissions.
   /// </summary>
   UNAUTHORIZED,

   /// <summary>
   ///     The request contained invalid arguments or parameters.
   /// </summary>
   INVALID_ARGUMENT,

   /// <summary>
   ///     The requested endpoint does not exist or is not accessible.
   /// </summary>
   ENDPOINT_NOT_FOUND,

   /// <summary>
   ///     The requested resource (e.g., database entity) was not found.
   /// </summary>
   RESOURCES_NOT_FOUND,

   /// <summary>
   ///     An unspecified internal server error occurred.
   ///     Indicates an unexpected error condition that needs investigation.
   /// </summary>
   INTERNAL_SERVER_ERROR,

   /// <summary>
   ///     The request body contained malformed JSON that could not be parsed.
   /// </summary>
   JSON_PARSE_ERROR,

   /// <summary>
   ///     The request payload exceeds the maximum allowed size limit.
   /// </summary>
   PAYLOAD_TOO_LARGE,

   /// <summary>
   ///     The request content type is not supported by the endpoint.
   /// </summary>
   UNSUPPORTED_MEDIA_TYPE,

   /// <summary>
   ///     The request was blocked by Cross-Origin Resource Sharing (CORS) policy.
   /// </summary>
   CORS_ERROR,

   /// <summary>
   ///     The request took too long to complete and timed out.
   /// </summary>
   REQUEST_TIMEOUT,

   /// <summary>
   ///     The request failed validation checks (e.g., model validation).
   /// </summary>
   VALIDATION_ERROR,

   /// <summary>
   ///     The client has exceeded their allowed request rate limit.
   /// </summary>
   RATE_LIMIT_EXCEEDED
}