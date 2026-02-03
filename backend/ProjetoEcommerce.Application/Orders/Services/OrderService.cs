using AutoMapper;
using ProjetoEcommerce.Application.Orders.DTOs.Requests;
using ProjetoEcommerce.Application.Orders.DTOs.Responses;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Events; // <--- NÃO ESQUEÇA DESTE USING
using ProjetoEcommerce.Domain.Enums;
using ProjetoEcommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IShippingRepository _shippingRepository;
        private readonly IUserRepository _userRepository;
        // 1. ADIÇÃO: Dependência do RabbitMQ
        private readonly IMessageBusService _messageBus;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IShippingRepository shippingRepository,
            IUserRepository userRepository,
            IMessageBusService messageBus, // <--- 2. INJEÇÃO NO CONSTRUTOR
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _shippingRepository = shippingRepository;
            _userRepository = userRepository;
            _messageBus = messageBus; // <--- ATRIBUIÇÃO
            _mapper = mapper;
        }

        public async Task<OrderResponse> CreateOrderAsync(Guid userId, CreateOrderRequest request)
        {
            var cart = await _cartRepository.GetByUserAsync(userId);

            if (cart == null || !cart.Items.Any())
                throw new Exception("Carrinho vazio ou não encontrado.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("Usuário não encontrado.");

            var order = new Order(
                userId,
                request.ShippingAddress,
                request.BillingAddress,
                cart.TotalAmount
            );

            foreach (var item in cart.Items)
            {
                order.AddItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity);
            }

            // Salva no Banco
            await _orderRepository.AddAsync(order);

            // Limpa o Carrinho
            cart.Clear();
            await _cartRepository.UpdateAsync(cart);

            // ============================================================
            // 3. AQUI ESTÁ O TRECHO QUE FALTAVA (O ENVIO PARA A FILA)
            // ============================================================
            try
            {
                var orderEvent = new OrderCreatedEvent(
                    order.Id,
                    user.Id,
                    user.Email,
                    $"{user.FirstName} {user.LastName}",
                    user.PhoneNumber,
                    order.TotalAmount
                );

                // Envia para o RabbitMQ
                _messageBus.Publish(orderEvent, "order-created");
            }
            catch (Exception ex)
            {
                // Loga erro mas não para o fluxo
                Console.WriteLine($"[ERRO RABBITMQ] Falha ao publicar: {ex.Message}");
            }
            return _mapper.Map<OrderResponse>(order);
        }

        // ... Resto dos métodos continua igual ...
        public async Task<OrderResponse> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Pedido não encontrado.");

            order.UpdateStatus(status);
            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderResponse>(order);
        }
    }
}