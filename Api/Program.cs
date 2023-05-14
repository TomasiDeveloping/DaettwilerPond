using Api.Configurations;
using Application.Interfaces;
using HealthChecks.UI.Client;
using Infrastructure.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Persistence.Logic;
using Persistence.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCors();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureHealthChecks();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureAutoMapper();

// Register custom services to the container
builder.Services.AddScoped<IWebHookService, WebHookService>();
builder.Services.AddScoped<ILsn50V2d20Logic, Lsn50V2d20Logic>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorTypeRepository, SensorTypeRepository>();
builder.Services.AddScoped<ILsn50V2MeasurementRepository, Lsn50V2MeasurementRepository>();
builder.Services.AddScoped<ILsn50V2LifecycleRepository, Lsn50V2LifecycleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.Run();
