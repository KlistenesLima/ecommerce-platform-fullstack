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

        // CORREÇÃO 1: Se não achar, cria um vazio e retorna (sem erro 404)
        public async Task<CartResponse> GetCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);

            if (cart == null)
            {
                // Cria o carrinho na memória
                cart = new ProjetoEcommerce.Domain.Entities.Cart(userId);

                // Salva no banco imediatamente para garantir que existe
                await _cartRepository.AddAsync(cart);
            }

            return _mapper.Map<CartResponse>(cart);
        }

        // CORREÇÃO 2: Se for adicionar item e não tiver carrinho, cria antes
        public async Task<CartResponse> AddToCartAsync(Guid userId, AddToCartRequest request)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);

            // Se o carrinho ainda não existe, cria agora (Lazy Loading)
            if (cart == null)
            {
                cart = new ProjetoEcommerce.Domain.Entities.Cart(userId);
                await _cartRepository.AddAsync(cart);
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null) throw new Exception("Produto não encontrado.");

            // Verifica se o item já existe no carrinho
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + request.Quantity);
            }
            else
            {
                cart.AddItem(product, request.Quantity);
            }

            await _cartRepository.UpdateAsync(cart);
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task RemoveFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);
            if (cart == null) return; // Se não tem carrinho, não tem o que remover

            cart.RemoveItem(productId);
            await _cartRepository.UpdateAsync(cart);
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);
            if (cart == null) return;

            cart.Clear();
            await _cartRepository.UpdateAsync(cart);
        }

        public async Task<CartResponse> UpdateQuantityAsync(Guid userId, UpdateQuantityRequest request)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);
            if (cart == null) throw new Exception("Carrinho não encontrado.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null) throw new Exception("Item não está no carrinho.");

            item.UpdateQuantity(request.Quantity);

            await _cartRepository.UpdateAsync(cart);
            return _mapper.Map<CartResponse>(cart);
        }
    }
}