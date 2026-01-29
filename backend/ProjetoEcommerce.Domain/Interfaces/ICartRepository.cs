using ProjetoEcommerce.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetByUserAsync(Guid userId);
        Task AddOrUpdateAsync(Cart cart);
    }
}