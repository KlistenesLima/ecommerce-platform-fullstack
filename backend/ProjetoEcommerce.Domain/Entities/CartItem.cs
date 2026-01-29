using System;

namespace ProjetoEcommerce.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        // Propriedades de Navegação
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }

        // Propriedade Calculada (Resolve o erro "TotalPrice")
        public decimal TotalPrice => UnitPrice * Quantity;

        protected CartItem() { }

        // Construtor que aceita o Objeto Product (Resolve o erro do construtor)
        public CartItem(Guid cartId, Product product, int quantity)
        {
            CartId = cartId;
            ProductId = product.Id;
            ProductName = product.Name;
            UnitPrice = product.Price;
            Quantity = quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }
    }
}
