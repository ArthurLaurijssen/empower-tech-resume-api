using API.Authentication;
using API.ErrorExtensions;
using API.Mapping.Profiles;
using API.Responses;
using AutoMapper;
using Azure.Storage.Blobs;
using BL.Interfaces.Developers;
using BL.Interfaces.DeveloperSkills;
using BL.Interfaces.Experiences;
using BL.Interfaces.Projects;
using BL.Interfaces.SocialMedia;
using BL.Interfaces.Users;
using BL.Managers.Developers;
using BL.Managers.DeveloperSkills;
using BL.Managers.Experiences;
using BL.Managers.Projects;
using BL.Managers.SocialMedia;
using BL.Managers.Users;
using DAL.Data;
using DAL.Interfaces;
using DAL.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Initialize the application builder
var builder = WebApplication.CreateBuilder(args);

// Configure API Controllers and Custom Validation Handling
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Suppress default client error mapping for custom handling
        options.SuppressMapClientErrors = true;

        // Custom validation response factory
        options.InvalidModelStateResponseFactory = context =>
        {
            // Aggregate validation errors into a single message
            var errors = string.Join(" | ", context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            // Create standardized error response
            var errorResponse = new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Error = "Validation Failed",
                Code = ErrorCode.VALIDATION_ERROR,
                Details = errors,
                Id = context.HttpContext.Request.Path.Value
            };

            // Configure and return JSON response
            return new JsonResult(errorResponse)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ContentType = "application/json"
            };
        };
    });

// Database Configuration
// Configure MySQL connection with auto-detected server version
builder.Services.AddDbContext<ResumeDbContext>(options =>
    options
        .UseMySql(
            builder.Configuration.GetConnectionString("MySQLConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQLConnection"))
        )
);

// Azure Blob Storage Configuration
builder.Services.AddScoped<IBlobService, BlobService>(serviceProvider =>
    new BlobService(new BlobServiceClient(
        builder.Configuration.GetConnectionString("AzureStorageConnection")
    ), serviceProvider.GetRequiredService<ILogger<BlobService>>())
);

// Authentication Configuration
builder.ConfigureAuth0(); // Custom Auth0 configuration extension

// Data Access Layer: Repository Registration
builder.Services.AddScoped<IDeveloperRepository, DeveloperRepository>();
builder.Services.AddScoped<IDeveloperSkillRepository, DeveloperSkillRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<ISocialMediaRepository, SocialMediaRepository>();

// Business Layer: Manager and Accessor Registration
builder.Services.AddScoped<IDeveloperManager, DeveloperManager>();
builder.Services.AddScoped<IDeveloperAccessor, DeveloperAccessor>();
builder.Services.AddScoped<IDeveloperSkillManager, DeveloperSkillManager>();
builder.Services.AddScoped<IDeveloperSkillAccessor, DeveloperSkillAccessor>();
builder.Services.AddScoped<IExperienceAccessor, ExperienceAccessor>();
builder.Services.AddScoped<IExperienceManager, ExperienceManager>();
builder.Services.AddScoped<IProjectManager, ProjectManager>();
builder.Services.AddScoped<IProjectAccessor, ProjectAccessor>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IPermissionManager, PermissionManager>();
builder.Services.AddScoped<ISocialMediaManager, SocialMediaManager>();

// API Documentation Setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Logging Configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// AutoMapper Configuration
builder.Services.AddAutoMapper(cfg =>
{
    // Register all entity-to-DTO mapping profiles
    cfg.AddProfile<DeveloperMappingProfile>();
    cfg.AddProfile<DeveloperSkillMappingProfile>();
    cfg.AddProfile<ExperienceMappingProfile>();
    cfg.AddProfile<ProjectMappingProfile>();
    cfg.AddProfile<SocialMediaMappingProfile>();
});

// Build the application
var app = builder.Build();

// Development Environment Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure HTTP Pipeline
app.UseHttpsRedirection(); // Force HTTPS
app.UseGlobalErrorHandling(); // Custom error handling middleware

// Authentication Pipeline
// Note: Order is important - Authentication must precede Authorization
app.UseAuthentication();
app.UseAuthorization();

// Enable Controllers
app.MapControllers();

// Validate AutoMapper Configuration
// Ensures all mapping profiles are valid at startup
var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();

// Start the application
app.Run();