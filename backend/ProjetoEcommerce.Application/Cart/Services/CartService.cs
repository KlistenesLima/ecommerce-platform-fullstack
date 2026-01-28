using AutoMapper;
using ProjetoEcommerce.Application.Cart.DTOs.Requests;
using ProjetoEcommerce.Application.Cart.DTOs.Responses;
using ProjetoEcommerce.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

// ALIAS: O Segredo para resolver o conflito
using DomainCart = ProjetoEcommerce.Domain.Entities.Cart;
using DomainCartItem = ProjetoEcommerce.Domain.Entities.CartItem;

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
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return null;
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<CartResponse> AddToCartAsync(Guid userId, AddToCartRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null) throw new Exception("Produto não encontrado.");

            // Usando o Alias DomainCart explicitamente
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new DomainCart(userId);
                await _cartRepository.AddAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                var newItem = new DomainCartItem(product.Id, product.Name, product.Price, request.Quantity);
                cart.Items.Add(newItem);
            }

            await _cartRepository.UpdateAsync(cart);
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task RemoveFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    await _cartRepository.UpdateAsync(cart);
                }
            }
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart != null)
            {
                cart.Items.Clear();
                await _cartRepository.UpdateAsync(cart);
            }
        }

        public async Task<CartResponse> UpdateQuantityAsync(Guid userId, UpdateQuantityRequest request)
        {
             var cart = await _cartRepository.GetByUserIdAsync(userId);
             if (cart == null) throw new Exception("Carrinho não encontrado");

             var item = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
             if (item != null)
             {
                 item.Quantity = request.Quantity;
                 await _cartRepository.UpdateAsync(cart);
             }
             return _mapper.Map<CartResponse>(cart);
        }
    }
}
