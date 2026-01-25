using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ProjetoEcommerce.Infra.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        public PaymentRepository(ApplicationDbContext context) => _context = context;
        public async Task<Payment> AddAsync(Payment payment) { _context.Payments.Add(payment); await _context.SaveChangesAsync(); return payment; }
        public async Task<Payment> GetByIdAsync(Guid id) => await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
        public async Task<Payment> GetByOrderIdAsync(Guid orderId) => await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        public async Task<IEnumerable<Payment>> GetAllAsync() => await _context.Payments.ToListAsync();
        public async Task UpdateAsync(Payment payment) { _context.Payments.Update(payment); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(Payment payment) { _context.Payments.Remove(payment); await _context.SaveChangesAsync(); }
    }
}
