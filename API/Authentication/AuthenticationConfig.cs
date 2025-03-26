using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Authentication;

public static class AuthenticationConfig
{
    public static void ConfigureAuth0(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Your Auth0 domain
                options.Authority = "https://auth.empowertech.be/";

                // Your API identifier
                options.Audience = "https://api.empowertech.be";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Auth0's default namespace for roles
                    RoleClaimType = "auth.empowertech.be/roles",

                    // Additional validation parameters
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.NoResult();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new { message = "Authentication failed" });
                        return context.Response.WriteAsync(result);
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new { message = "You are not authorized" });
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new
                            { message = "You are not authorized to access this resource" });
                        return context.Response.WriteAsync(result);
                    }
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            // Policy requiring Admin role
            options.AddPolicy("RequireAdminAccess", policy =>
                policy.RequireClaim("permissions", "Admin:access"));
        });
    }
}