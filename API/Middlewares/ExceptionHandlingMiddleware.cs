using System.Net;
using System.Text.Json;
using API.Responses;
using Microsoft.IdentityModel.Tokens;

namespace API.Middlewares;

/// <summary>
///     Middleware component that provides centralized exception handling for the application.
///     Converts exceptions into standardized API error responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    /// <summary>
    ///     JSON serializer options configured for error response serialization.
    ///     Uses camelCase property naming for consistent JSON formatting.
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Initializes a new instance of the ExceptionHandlingMiddleware.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline</param>
    /// <param name="logger">Logger for recording middleware activities and errors</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    ///     Processes HTTP requests and handles any exceptions that occur.
    /// </summary>
    /// <param name="context">The HTTP context for the current request</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <remarks>
    ///     Converts various exceptions into appropriate HTTP responses with standardized error formats.
    ///     Logs information about request processing and any errors that occur.
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Log incoming request information
            _logger.LogInformation("Processing request: {Method} {Path}",
                context.Request.Method, context.Request.Path);

            // Log presence of authorization header
            if (context.Request.Headers.ContainsKey("Authorization"))
                _logger.LogInformation("Authorization header present");

            // Process the request
            await _next(context);

            // Log completion status
            _logger.LogInformation("Request completed with status code: {StatusCode}",
                context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            // Log the exception details
            _logger.LogError(ex, "An unhandled exception occurred while processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

            // Map exception to appropriate error response
            var response = ex switch
            {
                // Security token validation failures
                SecurityTokenValidationException tokenEx => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Error = "Token validation failed",
                    Code = ErrorCode.TOKEN_VALIDATION_FAILED,
                    Details = tokenEx.Message
                },

                // Invalid GUID format errors
                FormatException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Error = "Invalid ID Format",
                    Code = ErrorCode.INVALID_ID_FORMAT,
                    Details = "The provided ID must be a valid GUID (format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)",
                    Id = context.Request.Path.Value?.Split('/').LastOrDefault()
                },

                // JSON parsing errors
                JsonException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Error = "Invalid JSON",
                    Code = ErrorCode.JSON_PARSE_ERROR,
                    Details = "The request body contains invalid JSON"
                },

                // Authorization failures
                UnauthorizedAccessException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Error = "Unauthorized",
                    Code = ErrorCode.UNAUTHORIZED,
                    Details = "You do not have permission to perform this action"
                },

                // Invalid argument errors
                ArgumentException argEx => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Error = "Invalid argument",
                    Code = ErrorCode.INVALID_ARGUMENT,
                    Details = argEx.Message
                },

                // Resource not found errors
                KeyNotFoundException keyNotFoundEx => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Error = "Resource Not Found",
                    Code = ErrorCode.RESOURCES_NOT_FOUND,
                    Details = keyNotFoundEx.Message
                },

                // Default/unexpected errors
                _ => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Error = "Internal Server Error",
                    Code = ErrorCode.INTERNAL_SERVER_ERROR,
                    Details = "An unexpected error occurred."
                }
            };

            // Set response properties
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            // Write error response
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        }
    }
}