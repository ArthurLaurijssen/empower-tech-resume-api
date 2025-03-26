using System.Text.Json;
using API.Middlewares;
using API.Responses;

namespace API.ErrorExtensions;

/// <summary>
///     Provides extension methods for configuring global error handling in the application pipeline.
/// </summary>
public static class ErrorHandlingExtensions
{
    /// <summary>
    ///     JSON serializer options configured for error response serialization.
    ///     Uses camelCase property naming for consistent JSON formatting.
    /// </summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    ///     Configures global error handling middleware for the application.
    /// </summary>
    /// <param name="app">The application builder instance</param>
    /// <returns>The application builder instance for method chaining</returns>
    /// <remarks>
    ///     This method sets up multiple middleware components to handle different types of errors:
    ///     - HTTP status code errors (4xx and 5xx)
    ///     - CORS policy violations
    ///     - Request body size limits
    ///     - Unhandled exceptions via ExceptionHandlingMiddleware
    /// </remarks>
    public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder app)
    {
        // Handle non-success status codes (4xx and 5xx)
        app.UseStatusCodePages(async context =>
        {
            var response = context.HttpContext.Response;

            // Only handle non-success and non-300s (redirects)
            // Check if response hasn't started to avoid multiple writes
            if (response.StatusCode >= 400 && response.HasStarted == false)
            {
                response.ContentType = "application/json";
                var errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    // Map status codes to human-readable error messages
                    Error = response.StatusCode switch
                    {
                        400 => "Bad Request",
                        401 => "Unauthorized",
                        403 => "Forbidden",
                        404 => "Endpoint Not Found",
                        405 => "Method Not Allowed",
                        413 => "Payload Too Large",
                        415 => "Unsupported Media Type",
                        429 => "Too Many Requests",
                        _ => "An error occurred"
                    },
                    // Map status codes to internal error codes
                    Code = response.StatusCode switch
                    {
                        400 => ErrorCode.INVALID_ARGUMENT,
                        401 => ErrorCode.UNAUTHORIZED,
                        403 => ErrorCode.UNAUTHORIZED,
                        404 => ErrorCode.ENDPOINT_NOT_FOUND,
                        405 => ErrorCode.INVALID_ARGUMENT,
                        413 => ErrorCode.PAYLOAD_TOO_LARGE,
                        415 => ErrorCode.UNSUPPORTED_MEDIA_TYPE,
                        429 => ErrorCode.RATE_LIMIT_EXCEEDED,
                        _ => ErrorCode.INTERNAL_SERVER_ERROR
                    },
                    Details = context.HttpContext.Request.Path.HasValue
                        ? $"Path: {context.HttpContext.Request.Path}"
                        : "No additional details available",
                    Id = context.HttpContext.Request.Path.Value
                };

                await response.WriteAsync(JsonSerializer.Serialize(errorResponse, JsonOptions));
            }
        });

        // Handle CORS policy violations
        app.Use(async (context, next) =>
        {
            context.Response.OnStarting(() =>
            {
                // Check for CORS errors (403 status with Origin header)
                if (context.Response.StatusCode == 403 &&
                    context.Request.Headers.ContainsKey("Origin"))
                {
                    context.Response.ContentType = "application/json";
                    var errorResponse = new ErrorResponse
                    {
                        StatusCode = 403,
                        Error = "CORS Error",
                        Code = ErrorCode.CORS_ERROR,
                        Details = "The request was blocked by CORS policy",
                        Id = context.Request.Path.Value
                    };

                    return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, JsonOptions));
                }

                return Task.CompletedTask;
            });

            await next();
        });

        // Handle request body size limit violations
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (BadHttpRequestException ex) when (ex.StatusCode == StatusCodes.Status413PayloadTooLarge)
            {
                // Clear any existing response
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
                context.Response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    StatusCode = 413,
                    Error = "Payload Too Large",
                    Code = ErrorCode.PAYLOAD_TOO_LARGE,
                    Details = "The request body exceeds the maximum allowed size",
                    Id = context.Request.Path.Value
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, JsonOptions));
            }
        });

        // Add custom exception handling middleware as final error handler
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}