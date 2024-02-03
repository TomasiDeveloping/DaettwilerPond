﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

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
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            // Adds security requirements for Bearer token authentication
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        Scheme = "Oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
}