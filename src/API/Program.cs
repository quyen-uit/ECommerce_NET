using API.Extensions;
using API.Middlewares;
using Asp.Versioning.ApiExplorer;
using Core;
using Core.Entities.Identity;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/api-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Use Serilog for logging
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAPIService(builder.Configuration);

// Add health checks
builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "postgresql",
        tags: new[] { "db", "sql", "postgres" });
    // .AddRedis( // Commented out - Redis disabled
    //     builder.Configuration.GetConnectionString("Redis")!,
    //     name: "redis",
    //     tags: new[] { "cache", "redis" });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("UserName", httpContext.User?.Identity?.Name);
        diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress);
    };
});

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"ECommerce API {description.GroupName.ToUpperInvariant()}");
        }
    });
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

// Basic CSRF protections for cookie-backed endpoints
app.UseMiddleware<API.Middlewares.SpaOriginValidationMiddleware>();
app.UseMiddleware<API.Middlewares.RefreshRateLimitMiddleware>();
app.UseMiddleware<API.Middlewares.BasketRateLimitMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Audit logging (after auth so we can capture user info)
app.UseMiddleware<API.Middlewares.AuditMiddleware>();

app.MapControllers();

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db") || check.Tags.Contains("cache"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false, // Only checks if app is running
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// init or update database
using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;
var context = service.GetRequiredService<ApplicationDbContext>();
var userManager = service.GetRequiredService<UserManager<AppUser>>();
var roleManager = service.GetRequiredService<RoleManager<AppRole>>();
var logger = service.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await ApplicationDbContextSeed.SeedAsync(context, userManager, roleManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration.");
}


app.Run();
