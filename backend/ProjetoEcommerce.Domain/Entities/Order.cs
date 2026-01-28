using ProjetoEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEcommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string ShippingAddress { get; private set; }
        public string BillingAddress { get; private set; }
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }
        
        // Lista de Itens
        public virtual List<OrderItem> Items { get; private set; } = new();

        // --- PROPRIEDADES DE NAVEGAÇÃO (LINKS VIRTUAIS) ---
        [ForeignKey("UserId")]
        public virtual User User { get; set; } // O AutoMapper precisa disso

        // Assumindo relação 1:1 ou 1:N, ajustaremos conforme seu Payment/ShippingEntity
        public virtual Payment Payment { get; set; }
        public virtual ShippingEntity Shipping { get; set; } 
        // --------------------------------------------------

        protected Order() { }

        public Order(Guid userId, string shippingAddress, string billingAddress)
        {
            UserId = userId;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Status = OrderStatus.Pending;
            TotalAmount = 0;
        }

        public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            var item = new OrderItem(productId, productName, unitPrice, quantity);
            Items.Add(item);
            TotalAmount += unitPrice * quantity;
        }

        public void UpdateStatus(OrderStatus status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
