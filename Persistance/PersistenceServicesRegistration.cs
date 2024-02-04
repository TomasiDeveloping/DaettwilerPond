using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Logic;
using Persistence.Repositories;

namespace Persistence;

// This class provides extension methods to configure and register persistence-related services in the Dependency Injection (DI) container.
public static class PersistenceServicesRegistration
{
    // Configures and registers persistence-related services in the DI container.
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {

        // Configure and register the DaettwilerPondDbContext for database access.
        services.AddDbContext<DaettwilerPondDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DaettwilerPondConnection"));
        });

        // Configure and register identity-related services for the User entity.
        services.AddIdentityCore<User>(options =>
            {
                // Set password policy requirements.
                options.Password.RequiredLength = 7;

                // Ensure unique email addresses for users.
                options.User.RequireUniqueEmail = true;

                // Configure account lockout settings.
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddRoles<UserRole>()
            .AddTokenProvider<EmailTokenProvider<User>>(TokenOptions.DefaultProvider)
            .AddEntityFrameworkStores<DaettwilerPondDbContext>();


        // Register scoped services for various repositories and logic classes.

        // Lsn50V2D20Logic provides logic related to Lsn50V2D20 sensors.
        services.AddScoped<ILsn50V2D20Logic, Lsn50V2D20Logic>();

        // SensorRepository handles CRUD operations for Sensor entities.
        services.AddScoped<ISensorRepository, SensorRepository>();

        // SensorTypeRepository handles CRUD operations for SensorType entities.
        services.AddScoped<ISensorTypeRepository, SensorTypeRepository>();

        // Lsn50V2MeasurementRepository handles CRUD operations for Lsn50V2Measurement entities.
        services.AddScoped<ILsn50V2MeasurementRepository, Lsn50V2MeasurementRepository>();

        // Lsn50V2LifecycleRepository handles CRUD operations for Lsn50V2Lifecycle entities.
        services.AddScoped<ILsn50V2LifecycleRepository, Lsn50V2LifecycleRepository>();

        // AuthenticationRepository handles authentication-related operations.
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

        // UserRepository handles CRUD operations for User entities.
        services.AddScoped<IUserRepository, UserRepository>();

        // FishTypeRepository handles CRUD operations for FishType entities.
        services.AddScoped<IFishTypeRepository, FishTypeRepository>();

        // FishingRegulationRepository handles CRUD operations for FishingRegulation entities.
        services.AddScoped<IFishingRegulationRepository, FishingRegulationRepository>();

        // AddressRepository handles CRUD operations for Address entities.
        services.AddScoped<IAddressRepository, AddressRepository>();

        // FishingLicenseRepository handles CRUD operations for FishingLicense entities.
        services.AddScoped<IFishingLicenseRepository, FishingLicenseRepository>();

        // FishingClubRepository handles CRUD operations for FishingClub entities.
        services.AddScoped<IFishingClubRepository, FishingClubRepository>();

        // CatchRepository handles CRUD operations for Catch entities.
        services.AddScoped<ICatchRepository, CatchRepository>();

        // CatchDetailRepository handles CRUD operations for CatchDetail entities.
        services.AddScoped<ICatchDetailRepository, CatchDetailRepository>();

        // Return the modified service collection.
        return services;
    }
}