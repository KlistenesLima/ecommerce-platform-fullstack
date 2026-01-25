using AutoMapper;
using ProjetoEcommerce.Application.Shipping.DTOs.Requests;
using ProjetoEcommerce.Application.Shipping.DTOs.Responses;
using ProjetoEcommerce.Application.Shippings.DTOs.Requests;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Enums;
using ProjetoEcommerce.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Shipping.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository _shippingRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public ShippingService(
            IShippingRepository shippingRepository,
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _shippingRepository = shippingRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<ShippingResponse> CreateShippingAsync(CreateShippingRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                throw new InvalidOperationException("Pedido não encontrado");

            if (!Enum.TryParse<ShippingProvider>(request.Provider, true, out var provider))
                throw new ArgumentException("Shipping provider inválido.");

            var shipping = new ShippingEntity(request.OrderId, request.Address, provider);
            shipping.SetTrackingNumber(GenerateTrackingNumber());
            shipping.UpdateStatus(ShippingStatus.Pending);

            var createdShipping = await _shippingRepository.AddAsync(shipping);
            return _mapper.Map<ShippingResponse>(createdShipping);
        }

        public async Task<ShippingResponse> GetShippingAsync(Guid id)
        {
            var shipping = await _shippingRepository.GetByIdAsync(id);
            return shipping == null ? null : _mapper.Map<ShippingResponse>(shipping);
        }

        public async Task<ShippingResponse> TrackShippingAsync(string trackingNumber)
        {
            var shipping = await _shippingRepository.GetByTrackingNumberAsync(trackingNumber);
            return shipping == null ? null : _mapper.Map<ShippingResponse>(shipping);
        }

        public async Task<bool> UpdateShippingStatusAsync(Guid id, ShippingStatus status)
        {
            var shipping = await _shippingRepository.GetByIdAsync(id);
            if (shipping == null) return false;

            shipping.UpdateStatus(status);
            await _shippingRepository.UpdateAsync(shipping);

            var order = await _orderRepository.GetByIdAsync(shipping.OrderId);
            if (order == null) return false;

            if (status == ShippingStatus.InTransit)
                order.UpdateStatus(OrderStatus.Shipped);
            else if (status == ShippingStatus.Delivered)
                order.UpdateStatus(OrderStatus.Delivered);

            await _orderRepository.UpdateAsync(order);
            return true;
        }

        private string GenerateTrackingNumber()
        {
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var random = Guid.NewGuid().ToString("N")[..8].ToUpper();
            return $"TRACK-{date}-{random}";
        }
    }
}
