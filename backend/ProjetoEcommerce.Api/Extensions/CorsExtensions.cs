using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjetoEcommerce.Api.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind CORS settings
        var corsSettings = new CorsSettings();
        configuration.GetSection("Cors").Bind(corsSettings);

        services.AddCors(options =>
        {
            // Development policy - Allow all
            options.AddPolicy("Development", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            // Production policy - Restricted
            options.AddPolicy("Production", policy =>
            {
                policy
                    .WithOrigins(corsSettings.AllowedOrigins)
                    .WithMethods(corsSettings.AllowedMethods)
                    .WithHeaders(corsSettings.AllowedHeaders)
                    .AllowCredentials();
            });

            // Custom policy based on configuration
            options.AddPolicy("Configured", policy =>
            {
                if (corsSettings.AllowAnyOrigin)
                {
                    policy.AllowAnyOrigin();
                }
                else
                {
                    policy.WithOrigins(corsSettings.AllowedOrigins);
                    if (corsSettings.AllowCredentials)
                    {
                        policy.AllowCredentials();
                    }
                }

                if (corsSettings.AllowAnyMethod)
                {
                    policy.AllowAnyMethod();
                }
                else
                {
                    policy.WithMethods(corsSettings.AllowedMethods);
                }

                if (corsSettings.AllowAnyHeader)
                {
                    policy.AllowAnyHeader();
                }
                else
                {
                    policy.WithHeaders(corsSettings.AllowedHeaders);
                }

                if (corsSettings.ExposedHeaders.Length > 0)
                {
                    policy.WithExposedHeaders(corsSettings.ExposedHeaders);
                }
            });
        });

        return services;
    }
}

public class CorsSettings
{
    public string[] AllowedOrigins { get; set; } = new[] { "http://localhost:3000", "http://localhost:5173" };
    public string[] AllowedMethods { get; set; } = new[] { "GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS" };
    public string[] AllowedHeaders { get; set; } = new[] { "Content-Type", "Authorization", "X-Requested-With" };
    public string[] ExposedHeaders { get; set; } = new[] { "Token-Expired", "X-Pagination" };
    public bool AllowAnyOrigin { get; set; } = false;
    public bool AllowAnyMethod { get; set; } = true;
    public bool AllowAnyHeader { get; set; } = true;
    public bool AllowCredentials { get; set; } = true;
}