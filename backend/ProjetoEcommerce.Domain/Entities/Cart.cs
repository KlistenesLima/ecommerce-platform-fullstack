using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoEcommerce.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; private set; }
        public DateTime? ExpiresAt { get; private set; }

        // Inicializa a lista para não dar NullReferenceException
        public virtual ICollection<CartItem> Items { get; private set; } = new List<CartItem>();
        public virtual User User { get; set; }

        protected Cart() { } // Para o EF Core

        // Construtor que usamos no Service
        public Cart(Guid userId)
        {
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddDays(7); // Carrinho expira em 7 dias, por exemplo
            Items = new List<CartItem>();
        }

        public void AddItem(Product product, int quantity)
        {
            Items.Add(new CartItem(this.Id, product, quantity));
        }

        public void RemoveItem(Guid productId)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null) Items.Remove(item);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    }
}