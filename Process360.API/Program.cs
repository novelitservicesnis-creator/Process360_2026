using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Repository.Interface;
using Process360.Repository.Repository;
using Process360.Repository.Repository.Base;
using Process360.API.Mappings;

using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens;
using System.Security.Authentication;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// ============================================================
// DATABASE CONFIGURATION
// ============================================================
var connectionString = "Server=DESKTOP-6QBF8V1\\SQLEXPRESS2022;Database=Process360;Integrated Security=true;TrustServerCertificate=true;";

builder.Services.AddDbContext<ProcessDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// ============================================================
// DEPENDENCY INJECTION - REPOSITORIES
// ============================================================

// Base Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

// Customer Repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Project Repository
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

// Resources Repository
builder.Services.AddScoped<IResourcesRepository, ResourcesRepository>();

// Project Task Type Repository
builder.Services.AddScoped<IProjectTaskTypeRepository, ProjectTaskTypeRepository>();

// Project Task Repository
builder.Services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();

// Project Task Attachments Repository
builder.Services.AddScoped<IProjectTaskAttachmentsRepository, ProjectTaskAttachmentsRepository>();

// Technology Repository
builder.Services.AddScoped<ITechnologyRepository, TechnologyRepository>();

// Project Planning Repository
builder.Services.AddScoped<IProjectPlanningRepository, ProjectPlanningRepository>();

// Project Planning Tasks Repository
builder.Services.AddScoped<IProjectPlanningTasksRepository, ProjectPlanningTasksRepository>();

// Project Resources Repository
builder.Services.AddScoped<IProjectResourcesRepository, ProjectResourcesRepository>();

// Task Comments Repository
builder.Services.AddScoped<ITaskCommentsRepository, TaskCommentsRepository>();

// Project Task Linked Repository
builder.Services.AddScoped<IProjectTaskLinkedRepository, ProjectTaskLinkedRepository>();

// Project Task Status History Repository
builder.Services.AddScoped<IProjectTaskStatusHistoryRepository, ProjectTaskStatusHistoryRepository>();

// ============================================================
// AUTOMAPPER CONFIGURATION
// ============================================================
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// ============================================================
// LOGGING CONFIGURATION
// ============================================================
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configure logging levels
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

// Add logging to appsettings
if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Debug);
}
else
{
    builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Information);
}

// ============================================================
// CONTROLLERS & API CONFIGURATION
// ============================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger/OpenAPI documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Process360 API", 
        Version = "v1",
        Description = "Process360 Project Management API"
    });
});

// ============================================================
// CORS CONFIGURATION
// ============================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7001",
                "https://localhost:3000",
                "https://process360.com",
                "https://*.process360.com"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add Authorization
builder.Services.AddAuthorization();

// ============================================================
// SWAGGER CONFIGURATION
// ============================================================

// ============================================================
// BUILD APPLICATION
// ============================================================
var app = builder.Build();

// ============================================================
// MIDDLEWARE CONFIGURATION
// ============================================================

// HTTPS Redirection
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS Policy
app.UseCors("AllowSpecificOrigins");

// Request logging middleware
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);

    await next();

    logger.LogInformation("Response: {StatusCode} {Method} {Path}", 
        context.Response.StatusCode, 
        context.Request.Method, 
        context.Request.Path);
});

// Authorization
app.UseAuthorization();

// Swagger UI middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Process360 API v1");
    c.RoutePrefix = string.Empty; // Serve Swagger UI at root
});

// Map Controllers
app.MapControllers();

// ============================================================
// DATABASE INITIALIZATION
// ============================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<ProcessDbContext>();

        // Apply any pending migrations
        context.Database.Migrate();

        logger.LogInformation("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database");
    }
}

var mainLogger = app.Services.GetRequiredService<ILogger<Program>>();
mainLogger.LogInformation("=== Application Starting ===");
mainLogger.LogInformation("Database: Process360");
mainLogger.LogInformation("Server: DESKTOP-6QBF8V1\\SQLEXPRESS2022");
mainLogger.LogInformation("Connection: Windows Authentication (Integrated Security)");
mainLogger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
mainLogger.LogInformation("API Running at: https://localhost:7001");
mainLogger.LogInformation("=== Startup Complete ===");

app.Run();
