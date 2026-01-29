using ProjetoEcommerce.Application.Shippings.DTOs.Requests;
using ProjetoEcommerce.Application.Shippings.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Shippings.Services
{
    public interface IShippingService
    {
        Task<decimal> CalculateShippingAsync(string zipCode, decimal weight);
        Task<ShippingResponse> GetShippingByIdAsync(Guid id);
        Task<ShippingResponse> CreateShippingAsync(CreateShippingRequest request);
        Task<ShippingResponse> UpdateStatusAsync(UpdateShippingStatusRequest request);
    }
}
