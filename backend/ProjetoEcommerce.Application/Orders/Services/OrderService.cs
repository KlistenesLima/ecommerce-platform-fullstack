using AutoMapper;
using ProjetoEcommerce.Application.Orders.DTOs.Requests;
using ProjetoEcommerce.Application.Orders.DTOs.Responses;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Enums;
using ProjetoEcommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> CreateOrderAsync(Guid userId, CreateOrderRequest request)
        {
            var cart = await _cartRepository.GetByIdAsync(request.CartId);
            if (cart == null || cart.Items.Count == 0)
                throw new InvalidOperationException("Carrinho vazio");

            decimal total = 0;
            foreach (var item in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new InvalidOperationException("Produto não encontrado");
                if (product.Stock < item.Quantity)
                    throw new InvalidOperationException("Stock insuficiente");
                total += product.Price * item.Quantity;
            }

            var order = new Order(userId, request.CartId, total);
            order.ShippingAddress = request.ShippingAddress;
            order.BillingAddress = request.BillingAddress;
            order.UpdateStatus(OrderStatus.Pending);

            var createdOrder = await _orderRepository.AddAsync(order);

            foreach (var item in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.DecreaseStock(item.Quantity);
                    await _productRepository.UpdateAsync(product);
                }
            }

            // Corrigido: passa cart.Id em vez de cart
            await _cartRepository.DeleteAsync(cart.Id);
            return _mapper.Map<OrderResponse>(createdOrder);
        }

        public async Task<OrderResponse?> GetOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? null : _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid id, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;
            order.UpdateStatus(status);
            await _orderRepository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> CancelOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;
            order.UpdateStatus(OrderStatus.Cancelled);
            await _orderRepository.UpdateAsync(order);
            return true;
        }
    }
}