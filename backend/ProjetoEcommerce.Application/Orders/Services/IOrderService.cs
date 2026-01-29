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
        Task<OrderResponse> CreateOrderAsync(Guid userId, CreateOrderRequest request);
        Task<OrderResponse> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(Guid userId);
        Task<OrderResponse> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    }
}
