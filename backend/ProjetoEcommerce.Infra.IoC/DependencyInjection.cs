using Amazon;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoEcommerce.Application.Storage;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Infra.Data.Context;
using ProjetoEcommerce.Infra.Data.Repositories;
using System;

namespace ProjetoEcommerce.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDatabaseConfiguration(configuration);

        // Repositories
        services.AddRepositories();

        // Cache (Redis)
        services.AddCacheServices(configuration);

        // Cloud Services (AWS S3)
        services.AddCloudServices(configuration);

        // Message Queue (Kafka & RabbitMQ)
        services.AddMessageQueueServices(configuration);

        return services;
    }

    private static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
                npgsqlOptions.CommandTimeout(30);
            }));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IShippingRepository, ShippingRepository>();

        return services;
    }

    private static IServiceCollection AddCacheServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind Redis settings
        var redisSettings = new RedisSettings();
        configuration.GetSection("Redis").Bind(redisSettings);
        services.AddSingleton(redisSettings);

        // StackExchange.Redis configuration
        if (!string.IsNullOrEmpty(redisSettings.ConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.ConnectionString;
                options.InstanceName = "EcommerceCache:";
            });
        }

        return services;
    }

    private static IServiceCollection AddCloudServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind AWS settings
        var awsSettings = new AWSSettings();
        configuration.GetSection("AWS").Bind(awsSettings);
        services.AddSingleton(awsSettings);

        // Configure AWS S3
        if (!string.IsNullOrEmpty(awsSettings.AccessKey) && !string.IsNullOrEmpty(awsSettings.SecretKey))
        {
            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(awsSettings.Region)
            };

            services.AddSingleton<IAmazonS3>(sp =>
                new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, s3Config));
        }
        else
        {
            // Usar credenciais do ambiente (IAM Role, etc)
            services.AddSingleton<IAmazonS3>(sp =>
                new AmazonS3Client(RegionEndpoint.GetBySystemName(awsSettings.Region)));
        }

        services.AddScoped<IS3StorageService, S3StorageService>();

        return services;
    }

    private static IServiceCollection AddMessageQueueServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind Kafka settings
        var kafkaSettings = new KafkaSettings();
        configuration.GetSection("Kafka").Bind(kafkaSettings);
        services.AddSingleton(kafkaSettings);

        // Bind RabbitMQ settings
        var rabbitSettings = new RabbitMQSettings();
        configuration.GetSection("RabbitMQ").Bind(rabbitSettings);
        services.AddSingleton(rabbitSettings);

        return services;
    }
}

#region Settings Classes

public class RedisSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public int DefaultExpirationMinutes { get; set; } = 60;
}

public class AWSSettings
{
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Region { get; set; } = "us-east-1";
    public string S3BucketName { get; set; } = string.Empty;
}

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public string GroupId { get; set; } = "ecommerce-group";
}

public class RabbitMQSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
}

#endregion
