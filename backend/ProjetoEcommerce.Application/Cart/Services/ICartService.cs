using ProjetoEcommerce.Application.Cart.DTOs.Requests;
using ProjetoEcommerce.Application.Cart.DTOs.Responses;
using System;
using System.Threading.Tasks;

public interface ICartService
{
    Task<CartResponse> GetByUserAsync(Guid userId);
    Task<CartResponse> AddItemAsync(AddToCartRequest request);
    Task<CartResponse> RemoveItemAsync(Guid cartId, Guid productId);
    Task<CartResponse> ClearAsync(Guid cartId);
    Task<CartResponse> UpdateItemQuantityAsync(Guid cartId, Guid productId, int quantity);
}
