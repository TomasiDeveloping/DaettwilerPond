using Application.Mappings;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

namespace Api.Configurations;

public static class ServiceExtensions
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DaettwilerPondDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DaettwilerPondConnection"));
        });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, UserRole>(options =>
            {
                options.Password.RequiredLength = 7;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddEntityFrameworkStores<DaettwilerPondDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(5);
        });
    }

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

    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
    }

    public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DaettwilerPondConnection")!)
            .AddDbContextCheck<DaettwilerPondDbContext>();
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfigurationSection jwtSection)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
            };
        });
    }

    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(options =>
        {
            options.AddProfile<SensorProfile>();
            options.AddProfile<SensorTypeProfile>();
            options.AddProfile<Lsn50V2LifecycleProfile>();
            options.AddProfile<Lsn50V2MeasurementProfile>();
            options.AddProfile<AuthenticationProfile>();
            options.AddProfile<UserProfile>();
            options.AddProfile<FishTypeProfile>();
            options.AddProfile<FishingRegulationProfile>();
        });
    }
}