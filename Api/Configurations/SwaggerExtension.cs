using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;

namespace Api.Configurations;

public static class SwaggerExtension
{
    // Configures Swagger for API documentation
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Defines API documentation details for version 1
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Dättwiler Pond API",
                Description = "API for Dättwiler Pond Portal",
                Version = "v1",
                License = new OpenApiLicense
                {
                    Name = "MIT License"
                },
                Contact = new OpenApiContact
                {
                    Name = "TomasiDeveloping",
                    Email = "info@tomasi-developing.ch",
                    Url = new Uri("https://tomasi-developing.ch")
                }
            });

            // Configures Bearer token authentication for Swagger
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            // Adds security requirements for Bearer token authentication
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, document)] = []
            });
        });
    }
}