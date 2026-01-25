namespace ProjetoEcommerce.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual CartEntity Cart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

        public CartItem() { }

        public CartItem(Guid cartId, Guid productId, int quantity, decimal unitPrice)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public decimal Total => Quantity * UnitPrice;
    }
}