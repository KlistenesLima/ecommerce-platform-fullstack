using ProjetoEcommerce.Application.Cart.DTOs.Requests;
using ProjetoEcommerce.Application.Cart.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Cart.Services
{
    public interface ICartService
    {
        Task<CartResponse> GetCartAsync(Guid userId);
        Task<CartResponse> AddToCartAsync(Guid userId, AddToCartRequest request);
        Task RemoveFromCartAsync(Guid userId, Guid productId);
        Task ClearCartAsync(Guid userId);
        Task<CartResponse> UpdateQuantityAsync(Guid userId, UpdateQuantityRequest request);
    }
}
