using Api.Configurations;
using Application;
using Application.Models;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.BackgroundJobs;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Persistence;
using Quartz;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("ApplicationName", "DättwilerWeiher");
});

// Configure Quartz
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

// Register Projects
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

// Register custom services to the container
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailSettings"));


var app = builder.Build();

try
{
    Log.Logger.Information("Starting web host");
    // Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors("CorsPolicy");

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });


    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}