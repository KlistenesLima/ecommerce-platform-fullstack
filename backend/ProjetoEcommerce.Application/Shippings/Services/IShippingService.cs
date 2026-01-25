using ProjetoEcommerce.Application.Shipping.DTOs.Requests;
using ProjetoEcommerce.Application.Shipping.DTOs.Responses;
using ProjetoEcommerce.Application.Shippings.DTOs.Requests;
using ProjetoEcommerce.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Shipping.Services
{
    public interface IShippingService
    {
        Task<ShippingResponse> CreateShippingAsync(CreateShippingRequest request);
        Task<ShippingResponse> GetShippingAsync(Guid id);
        Task<ShippingResponse> TrackShippingAsync(string trackingNumber);
        Task<bool> UpdateShippingStatusAsync(Guid id, ShippingStatus status);
    }
}
