using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Domain.Interfaces
{
    public interface ICartRepository
    {
        // Uso explícito de Domain.Entities.Cart para evitar ambiguidade com Interfaces.Cart
        Task<ProjetoEcommerce.Domain.Entities.Cart> GetByIdAsync(Guid id);
        Task<ProjetoEcommerce.Domain.Entities.Cart> GetByUserIdAsync(Guid userId);
        Task AddAsync(ProjetoEcommerce.Domain.Entities.Cart cart);
        Task UpdateAsync(ProjetoEcommerce.Domain.Entities.Cart cart);
        Task DeleteAsync(Guid id);
    }
}
