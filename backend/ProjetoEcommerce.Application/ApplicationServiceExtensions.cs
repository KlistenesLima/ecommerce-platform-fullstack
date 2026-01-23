using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoEcommerce.Application.Auth.Services;
using ProjetoEcommerce.Application.Orders.Services;
using ProjetoEcommerce.Application.Payments.Services;
using ProjetoEcommerce.Application.Shipping.Services;
using ProjetoEcommerce.Application.Users.Services;
using System;
using System.Reflection;

namespace ProjetoEcommerce.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapperConfiguration();

        // FluentValidation
        services.AddValidationConfiguration();

        // Application Services
        services.AddApplicationServices();

        return services;
    }

    private static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.AllowNullDestinationValues = true;
        }, AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }

    private static IServiceCollection AddValidationConfiguration(this IServiceCollection services)
    {
        // FluentValidation - registra todos os validators do assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IShippingService, ShippingService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}