using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper and Profiles
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
            options.AddProfile<AddressProfile>();
            options.AddProfile<FishingLicenseProfile>();
            options.AddProfile<FishingClubProfile>();
        });

        return services;
    }
}