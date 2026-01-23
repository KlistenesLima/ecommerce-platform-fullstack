using ProjetoEcommerce.Domain.Entities;

namespace ProjetoEcommerce.Domain.Interfaces
{
    public interface IShippingRepository
    {
        Task<ShippingEntity> AddAsync(ShippingEntity shipping);
        Task<ShippingEntity> GetByIdAsync(Guid id);
        Task<ShippingEntity> GetByOrderIdAsync(Guid orderId);
        Task<ShippingEntity> GetByTrackingNumberAsync(string trackingNumber);
        Task<IEnumerable<ShippingEntity>> GetAllAsync();
        Task UpdateAsync(ShippingEntity shipping);
        Task DeleteAsync(ShippingEntity shipping);
    }
}
