using ProjetoEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoEcommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string ShippingAddress { get; private set; }
        public string BillingAddress { get; private set; }
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }
        
        // Relacionamentos
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();
        public virtual Payment Payment { get; set; }
        public virtual ShippingEntity Shipping { get; set; }

        protected Order() { } // Necessário para o EF Core

        // O Construtor que o Service está chamando
        public Order(Guid userId, string shippingAddress, string billingAddress, decimal totalAmount)
        {
            UserId = userId;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            TotalAmount = totalAmount;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            Items.Add(new OrderItem(this.Id, productId, productName, unitPrice, quantity));
        }

        public void UpdateStatus(OrderStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
