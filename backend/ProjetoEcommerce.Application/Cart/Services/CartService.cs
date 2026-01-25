using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Application.Cart.DTOs.Requests;
using ProjetoEcommerce.Application.Cart.DTOs.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Cart.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<CartResponse> GetByUserAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);
            if (cart == null)
            {
                cart = new CartEntity(userId);
                await _cartRepository.AddItemAsync(cart);
            }

            return MapToResponse(cart);
        }

        public async Task<CartResponse> AddItemAsync(AddToCartRequest request)
        {
            var cart = await _cartRepository.GetByIdAsync(request.CartId)
                       ?? throw new Exception("Carrinho não encontrado");

            var product = await _productRepository.GetByIdAsync(request.ProductId)
                          ?? throw new Exception("Produto não encontrado");

            if (product.Stock < request.Quantity)
                throw new Exception("Quantidade insuficiente em estoque");

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price
                });
            }

            await _cartRepository.UpdateAsync(cart);
            return MapToResponse(cart);
        }

        public async Task<CartResponse> RemoveItemAsync(Guid cartId, Guid productId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId)
                       ?? throw new Exception("Carrinho não encontrado");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId)
                       ?? throw new Exception("Item não encontrado no carrinho");

            cart.Items.Remove(item);
            await _cartRepository.UpdateAsync(cart);

            return MapToResponse(cart);
        }

        public async Task<CartResponse> ClearAsync(Guid cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId)
                       ?? throw new Exception("Carrinho não encontrado");

            cart.Items.Clear();
            await _cartRepository.UpdateAsync(cart);

            return MapToResponse(cart);
        }

        public async Task<CartResponse> UpdateItemQuantityAsync(Guid cartId, Guid productId, int quantity)
        {
            if (quantity <= 0)
                return await RemoveItemAsync(cartId, productId);

            var cart = await _cartRepository.GetByIdAsync(cartId)
                       ?? throw new Exception("Carrinho não encontrado");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId)
                       ?? throw new Exception("Item não encontrado no carrinho");

            item.Quantity = quantity;
            await _cartRepository.UpdateAsync(cart);

            return MapToResponse(cart);
        }

        private CartResponse MapToResponse(CartEntity cart)
        {
            return new CartResponse
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.TotalPrice,
                CreatedAt = cart.CreatedAt
            };
        }
    }
}