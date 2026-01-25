using ProjetoEcommerce.Domain.Entities;

namespace ProjetoEcommerce.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> AddAsync(Payment payment);
        Task<Payment> GetByIdAsync(Guid id);
        Task<Payment> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(Payment payment);
    }
}
