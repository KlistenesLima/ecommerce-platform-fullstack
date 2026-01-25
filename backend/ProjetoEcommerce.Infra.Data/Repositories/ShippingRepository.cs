using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ProjetoEcommerce.Infra.Data.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly ApplicationDbContext _context;
        public ShippingRepository(ApplicationDbContext context) => _context = context;
        public async Task<ShippingEntity> AddAsync(ShippingEntity shipping) { _context.Shippings.Add(shipping); await _context.SaveChangesAsync(); return shipping; }
        public async Task<ShippingEntity> GetByIdAsync(Guid id) => await _context.Shippings.FirstOrDefaultAsync(s => s.Id == id);
        public async Task<ShippingEntity> GetByOrderIdAsync(Guid orderId) => await _context.Shippings.FirstOrDefaultAsync(s => s.OrderId == orderId);
        public async Task<ShippingEntity> GetByTrackingNumberAsync(string trackingNumber) => await _context.Shippings.FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);
        public async Task<IEnumerable<ShippingEntity>> GetAllAsync() => await _context.Shippings.ToListAsync();
        public async Task UpdateAsync(ShippingEntity shipping) { _context.Shippings.Update(shipping); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(ShippingEntity shipping) { _context.Shippings.Remove(shipping); await _context.SaveChangesAsync(); }
    }
}
