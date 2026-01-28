using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEcommerce.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        // --- PROPRIEDADE DE NAVEGAÇÃO ---
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } 
        // -------------------------------

        protected OrderItem() { }

        public OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}
