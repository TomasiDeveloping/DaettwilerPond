using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

// Static class for configuring and registering application services
public static class ApplicationServicesRegistration
{
    // Extension method to configure application services
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper and corresponding mapping profiles
        services.AddAutoMapper(options =>
        {
            // Adding mapping profiles for different entities and DTOs
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
            options.AddProfile<CatchProfile>();
            options.AddProfile<CatchDetailProfile>();
            options.AddProfile<CatchReportProfile>();
        });

        return services;
    }
}