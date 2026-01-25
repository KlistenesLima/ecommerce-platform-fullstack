using AutoMapper;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Application.Users.DTOs.Requests;
using ProjetoEcommerce.Application.Users.DTOs.Responses;
using ProjetoEcommerce.Application.Products.DTOs.Requests;
using ProjetoEcommerce.Application.Products.DTOs.Responses;
using ProjetoEcommerce.Application.Orders.DTOs.Requests;
using ProjetoEcommerce.Application.Orders.DTOs.Responses;
using ProjetoEcommerce.Application.Cart.DTOs.Requests;
using ProjetoEcommerce.Application.Cart.DTOs.Responses;
using System;

namespace ProjetoEcommerce.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mappings
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
            CreateMap<User, UserResponse>();

            // Product Mappings
            CreateMap<CreateProductRequest, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
            CreateMap<Product, ProductResponse>();

            // Order Mappings
            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Cart Mappings
            CreateMap<AddToCartRequest, CartItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));
            CreateMap<CartEntity, CartResponse>();
        }
    }
}