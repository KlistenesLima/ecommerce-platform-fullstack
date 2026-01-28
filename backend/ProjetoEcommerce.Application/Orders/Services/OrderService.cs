using AutoMapper;
using ProjetoEcommerce.Application.Orders.DTOs.Requests;
using ProjetoEcommerce.Application.Orders.DTOs.Responses;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Enums;
using ProjetoEcommerce.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

// ALIAS IMPORTANTE: Resolve conflito entre Namespace Cart e Classe Cart
using DomainCart = ProjetoEcommerce.Domain.Entities.Cart;

namespace ProjetoEcommerce.Application.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository; // Interface
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<System.Collections.Generic.IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<System.Collections.Generic.IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse> GetOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<OrderResponse> CreateOrderAsync(Guid userId, CreateOrderRequest request)
        {
            // Busca o Carrinho
            var cart = await _cartRepository.GetByIdAsync(request.CartId);

            if (cart == null)
            {
                cart = await _cartRepository.GetByUserIdAsync(userId);
            }

            if (cart == null)
            {
                throw new InvalidOperationException($"Nenhum carrinho encontrado para o usuário {userId}.");
            }

            if (!cart.Items.Any())
            {
                throw new InvalidOperationException("O carrinho está vazio.");
            }

            // Cria o Pedido (Domain Logic)
            var order = new Order(userId, request.ShippingAddress, request.BillingAddress);
            
            foreach (var item in cart.Items)
            {
                order.AddItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity);
            }
            
            await _orderRepository.AddAsync(order);
            
            // Opcional: Limpar carrinho
            // cart.Items.Clear();
            // await _cartRepository.UpdateAsync(cart);

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                order.UpdateStatus(status);
                await _orderRepository.UpdateAsync(order);
            }
        }
    }
}
