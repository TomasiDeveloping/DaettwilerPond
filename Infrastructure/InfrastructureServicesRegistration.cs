using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
    {
        // QuestPDF License configuration
        QuestPDF.Settings.License = LicenseType.Community;

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IWebHookService, WebHookService>();
        services.AddScoped<IPdfService, PdfService>();

        return services;
    }
}