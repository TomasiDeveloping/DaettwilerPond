using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Logic;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceServicesRegistration
{
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DaettwilerPondDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DaettwilerPondConnection"));
        });

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

        services.AddScoped<ILsn50V2d20Logic, Lsn50V2d20Logic>();
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<ISensorTypeRepository, SensorTypeRepository>();
        services.AddScoped<ILsn50V2MeasurementRepository, Lsn50V2MeasurementRepository>();
        services.AddScoped<ILsn50V2LifecycleRepository, Lsn50V2LifecycleRepository>();

        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFishTypeRepository, FishTypeRepository>();
        services.AddScoped<IFishingRegulationRepository, FishingRegulationRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IFishingLicenseRepository, FishingLicenseRepository>();

        return services;
    }
}