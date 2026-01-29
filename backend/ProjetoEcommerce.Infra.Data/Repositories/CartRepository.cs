using Microsoft.EntityFrameworkCore;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Infra.Data.Context;
using System;
using System.Linq;
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
            return await _dbSet
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddOrUpdateAsync(Cart cart)
        {
            var existingCart = await _dbSet
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            if (existingCart == null)
            {
                await _dbSet.AddAsync(cart);
            }

            // Salva/atualiza os itens diretamente
            foreach (var item in cart.Items)
            {
                var existingItem = await _context.Set<CartItem>()
                    .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == item.ProductId);

                if (existingItem == null)
                {
                    await _context.Set<CartItem>().AddAsync(item);
                }
                else
                {
                    existingItem.UpdateQuantity(item.Quantity);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}