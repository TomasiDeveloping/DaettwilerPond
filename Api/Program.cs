using Api.Configurations;
using Application;
using Application.Models;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.BackgroundJobs;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Persistence;
using Quartz;
using Serilog;

// Configure Serilog logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
// Add Serilog to services
builder.Services.AddSerilog(configureLogger =>
{
    configureLogger.ReadFrom.Configuration(builder.Configuration);
});

// Configure Quartz for background jobs
builder.Services.AddQuartz(options =>
{
    var jobKey = new JobKey(nameof(CheckFishingDayHasCompleted));

    options.AddJob<CheckFishingDayHasCompleted>(jobKey)
        .AddTrigger(
            trigger => trigger.ForJob(jobKey)
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 0)));
});


// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
builder.Services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

// Register Projects (Persistence, Application, Infrastructure)
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration.GetSection("Jwt"));

// Register custom services to the container (Email configuration)
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

// Build the application
var app = builder.Build();

try
{
    // Log information about starting the web host
    Log.Logger.Information("Starting web host");

    // Enable Swagger and Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();

    // Enable Cross-Origin Resource Sharing (CORS)
    app.UseCors("CorsPolicy");

    // Log HTTP requests using Serilog
    app.UseSerilogRequestLogging();

    // Redirect HTTP to HTTPS
    app.UseHttpsRedirection();

    // Enable authentication and authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Map controllers
    app.MapControllers();

    app.UseStaticFiles();

    var uploadsDirectory = Path.Combine(app.Environment.WebRootPath, "uploads", "images");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }


    app.UseStaticFiles(new StaticFileOptions()
    {
        RequestPath = "/images",
        FileProvider = new PhysicalFileProvider(uploadsDirectory)
    });

    // Map health checks endpoint with UI response
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    // Run the application
    app.Run();
}
catch (Exception e)
{
    // Log fatal error if the host terminates unexpectedly
    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    // Close and flush the Serilog log
    Log.CloseAndFlush();
}