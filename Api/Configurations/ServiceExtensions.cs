using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace Api.Configurations;

public static class ServiceExtensions
{
    // Configures Cross-Origin Resource Sharing (CORS) policies
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", p => p
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());
        });
    }

    // Configures API versioning
    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            // Sets default API version to 1.0
            options.DefaultApiVersion = new ApiVersion(1, 0);
            // Assumes default version when unspecified
            options.AssumeDefaultVersionWhenUnspecified = true;
            // Reports API versions in responses
            options.ReportApiVersions = true;
        });
    }

    // Configures health checks for SQL Server and DbContext
    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DaettwilerPondConnection")!)
            .AddDbContextCheck<DaettwilerPondDbContext>();
    }

    // Configures JWT authentication
    public static void ConfigureAuthentication(this IServiceCollection services, IConfigurationSection jwtSection)
    {
        // Sets default authentication scheme to JWT Bearer
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Validates issuer, audience, lifetime, and signing key
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                // Sets valid issuer, audience, and symmetric signing key
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
            };
        });
    }
}