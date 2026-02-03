using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoEcommerce.Application.Auth.Services;
using ProjetoEcommerce.Application.Cart.Services;
using ProjetoEcommerce.Application.Categories.Services;
using ProjetoEcommerce.Application.Orders.Services;
using ProjetoEcommerce.Application.Payments.Services;
using ProjetoEcommerce.Application.Products.Services;
using ProjetoEcommerce.Application.Shippings.Services;
using ProjetoEcommerce.Application.Storage;
using ProjetoEcommerce.Application.Users.Services;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Infra.Data.Context;
using ProjetoEcommerce.Infra.Data.Repositories;
using ProjetoEcommerce.Infra.MessageQueue.RabbitMQ;
using ProjetoEcommerce.Infra.Cloud.Email;

namespace ProjetoEcommerce.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IShippingRepository, ShippingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IShippingService, ShippingService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IS3StorageService, S3StorageService>();
            services.AddScoped<IEmailService, SmtpEmailService>();

            // --- RABBITMQ CONFIG ---
            // 1. Registra a implementação concreta para a interface específica
            services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

            // 2. Encaminha IMessageBusService para usar a MESMA instância de IRabbitMQPublisher
            // Isso garante que OrderService (que pede IMessageBusService) funcione.
            services.AddSingleton<IMessageBusService>(sp => sp.GetRequiredService<IRabbitMQPublisher>());

            return services;
        }
    }
}
