using AutoMapper;
using ProjetoEcommerce.Application.Cart.DTOs.Requests;
using ProjetoEcommerce.Application.Cart.DTOs.Responses;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Cart.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<CartResponse> GetCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);

            if (cart == null)
            {
                cart = new ProjetoEcommerce.Domain.Entities.Cart(userId);
                await _cartRepository.AddOrUpdateAsync(cart);
            }

            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<CartResponse> AddToCartAsync(Guid userId, AddToCartRequest request)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);

            if (cart == null)
            {
                cart = new ProjetoEcommerce.Domain.Entities.Cart(userId);
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new Exception("Produto não encontrado.");

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + request.Quantity);
            }
            else
            {
                cart.AddItem(product, request.Quantity);
            }

            await _cartRepository.AddOrUpdateAsync(cart);

            return _mapper.Map<CartResponse>(cart);
        }

        public async Task RemoveFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);

            if (cart == null)
                return;

            cart.RemoveItem(productId);
            await _cartRepository.AddOrUpdateAsync(cart);
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);

            if (cart == null)
                return;

            cart.Clear();
            await _cartRepository.AddOrUpdateAsync(cart);
        }

        public async Task<CartResponse> UpdateQuantityAsync(Guid userId, UpdateQuantityRequest request)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);

            if (cart == null)
                throw new Exception("Carrinho não encontrado.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (item == null)
                throw new Exception("Item não está no carrinho.");

            item.UpdateQuantity(request.Quantity);
            await _cartRepository.AddOrUpdateAsync(cart);

            return _mapper.Map<CartResponse>(cart);
        }
    }
}