using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Infrastructure;

// Extension methods to configure and register infrastructure-related services in the DI container.
public static class InfrastructureServicesRegistration
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
    {
        // Configures and registers infrastructure services in the DI container.
        QuestPDF.Settings.License = LicenseType.Community;

        // Registering infrastructure services with the DI container

        // Scoped services are created once per request.
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IWebHookService, WebHookService>();

        // Transient services are created every time they are requested.
        services.AddTransient<IPdfService, PdfService>();
        services.AddTransient<ISwissQrBillService, SwissQrBillService>();
        services.AddTransient<IReportService, ReportService>();

        return services;
    }
}