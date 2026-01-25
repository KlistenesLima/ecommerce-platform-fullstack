using ProjetoEcommerce.Domain.Entities;

namespace ProjetoEcommerce.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<CartEntity?> GetByIdAsync(Guid id);
        Task<CartEntity?> GetByUserAsync(Guid userId);
        Task<CartEntity> AddAsync(CartEntity cart);
        Task<CartEntity> AddItemAsync(CartEntity cart);
        Task<CartEntity> UpdateAsync(CartEntity cart);
        Task<bool> DeleteAsync(Guid id);
    }
}