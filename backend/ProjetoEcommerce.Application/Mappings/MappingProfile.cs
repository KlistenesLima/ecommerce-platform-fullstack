using AutoMapper;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Application.Products.DTOs.Requests;
using ProjetoEcommerce.Application.Products.DTOs.Responses;
using ProjetoEcommerce.Application.Categories.DTOs;
using ProjetoEcommerce.Application.Auth.DTOs.Requests;
using ProjetoEcommerce.Application.Auth.DTOs.Responses;
using ProjetoEcommerce.Application.Cart.DTOs.Responses;
using ProjetoEcommerce.Application.Orders.DTOs.Responses;
using ProjetoEcommerce.Application.Users.DTOs.Responses;
using ProjetoEcommerce.Application.Shippings.DTOs.Responses;
using ProjetoEcommerce.Application.Payments.DTOs.Responses;

// ALIAS OBRIGATÓRIO: Diferencia a classe 'Cart' do namespace 'Cart'
using DomainCart = ProjetoEcommerce.Domain.Entities.Cart;

namespace ProjetoEcommerce.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Products
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // Categories
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CreateCategoryRequest, Category>();

            // Users
            CreateMap<CreateUserRequest, User>();
            CreateMap<User, AuthResponse>();
            CreateMap<User, UserResponse>();

            // Cart (Usa o Alias DomainCart)
            CreateMap<DomainCart, CartResponse>();
            
            // CartItem
            CreateMap<CartItem, CartItemResponse>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.UnitPrice * src.Quantity));

            // Orders
            CreateMap<Order, OrderResponse>();
            CreateMap<OrderItem, OrderItemResponse>();

            // Shippings
            CreateMap<ShippingEntity, ShippingResponse>();

            // Payments
            CreateMap<Payment, PaymentResponse>();
        }
    }
}
