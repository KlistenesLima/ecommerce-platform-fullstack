using AutoMapper;
using ProjetoEcommerce.Application.Shippings.DTOs.Requests;
using ProjetoEcommerce.Application.Shippings.DTOs.Responses;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Enums;
using ProjetoEcommerce.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Shippings.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository _shippingRepository;
        private readonly IMapper _mapper;

        public ShippingService(IShippingRepository shippingRepository, IMapper mapper)
        {
            _shippingRepository = shippingRepository;
            _mapper = mapper;
        }

        public Task<decimal> CalculateShippingAsync(string zipCode, decimal weight)
        {
            // Lógica simulada
            var cost = 15.00m + (weight * 0.5m);
            return Task.FromResult(cost);
        }

        public async Task<ShippingResponse> GetShippingByIdAsync(Guid id)
        {
            var shipping = await _shippingRepository.GetByIdAsync(id);
            return _mapper.Map<ShippingResponse>(shipping);
        }

        public async Task<ShippingResponse> CreateShippingAsync(CreateShippingRequest request)
        {
            var shipping = new ShippingEntity
            {
                OrderId = request.OrderId,
                Provider = request.Provider,
                TrackingNumber = Guid.NewGuid().ToString().Substring(0, 10).ToUpper(),
                Status = ShippingStatus.Pending,
                Cost = 15.00m,
                EstimatedDelivery = DateTime.UtcNow.AddDays(5)
            };

            await _shippingRepository.AddAsync(shipping);
            return _mapper.Map<ShippingResponse>(shipping);
        }

        public async Task<ShippingResponse> UpdateStatusAsync(UpdateShippingStatusRequest request)
        {
            var shipping = await _shippingRepository.GetByIdAsync(request.ShippingId);
            if (shipping == null) throw new Exception("Envio não encontrado");

            shipping.Status = request.Status;
            
            await _shippingRepository.UpdateAsync(shipping);
            return _mapper.Map<ShippingResponse>(shipping);
        }
    }
}
