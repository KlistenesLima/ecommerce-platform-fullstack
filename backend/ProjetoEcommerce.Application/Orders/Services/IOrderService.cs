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
        Task<OrderResponse> GetOrderAsync(Guid id);
        Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(Guid userId);
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<bool> UpdateOrderStatusAsync(Guid id, OrderStatus status);
        Task<bool> CancelOrderAsync(Guid id);
    }
}
