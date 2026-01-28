using Microsoft.EntityFrameworkCore;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Infra.Data.Context;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Infra.Data.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetByUserAsync(Guid userId)
        {
            // O Include é vital para trazer os itens junto com o carrinho
            return await _dbSet
                .Include(c => c.Items) 
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
