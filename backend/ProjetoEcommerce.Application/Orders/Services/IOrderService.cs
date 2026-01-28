using ProjetoEcommerce.Application.Orders.DTOs.Requests;
using ProjetoEcommerce.Application.Orders.DTOs.Responses;
using ProjetoEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Orders.Services
{
    public interface IOrderService
    {
        // Alinhado com OrderService.GetAllAsync
        Task<IEnumerable<OrderResponse>> GetAllAsync();

        // Alinhado com OrderService.GetOrderAsync
        Task<OrderResponse> GetOrderAsync(Guid id);

        // Alinhado com OrderService.CreateOrderAsync
        Task<OrderResponse> CreateOrderAsync(Guid userId, CreateOrderRequest request);

        // Alinhado com OrderService.UpdateOrderStatusAsync (Task void)
        Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        
        // Se precisar de GetUserOrdersAsync no futuro, implemente no Service primeiro.
        // Por enquanto, removemos da interface para compilar.
    }
}
