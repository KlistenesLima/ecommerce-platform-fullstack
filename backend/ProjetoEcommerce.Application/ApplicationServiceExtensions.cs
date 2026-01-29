using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoEcommerce.Application.Auth.Services;
using ProjetoEcommerce.Application.Cart.Services;
using ProjetoEcommerce.Application.Categories.Services;
using ProjetoEcommerce.Application.Configuration;
using ProjetoEcommerce.Application.Mappings;
using ProjetoEcommerce.Application.Orders.Services;
using ProjetoEcommerce.Application.Payments.Services;
using ProjetoEcommerce.Application.Products.Services;
// CORREÇÃO: Usando o namespace PLURAL correto
using ProjetoEcommerce.Application.Shippings.Services; 
using ProjetoEcommerce.Application.Storage;
using ProjetoEcommerce.Application.Users.Services;
using ProjetoEcommerce.Application.Validations;

namespace ProjetoEcommerce.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            
            // O tipo agora será encontrado corretamente
            services.AddScoped<IShippingService, ShippingService>();

            services.Configure<AWSSettings>(settings =>
            {
                var section = configuration.GetSection("AWS");
                settings.AccessKey = section["AccessKey"];
                settings.SecretKey = section["SecretKey"];
                settings.ServiceUrl = section["ServiceUrl"];
                settings.S3BucketName = section["S3BucketName"];
            });

            services.AddScoped<IS3StorageService, S3StorageService>();

            return services;
        }
    }
}
