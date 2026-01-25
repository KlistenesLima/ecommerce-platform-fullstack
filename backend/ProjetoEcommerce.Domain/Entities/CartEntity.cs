namespace ProjetoEcommerce.Domain.Entities
{
    public class CartEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal TotalPrice => Items.Sum(item => item.Quantity * item.UnitPrice);

        public CartEntity() { }

        public CartEntity(Guid userId)
        {
            UserId = userId;
        }

        public decimal GetTotal()
        {
            return TotalPrice;
        }

        public void Clear()
        {
            Items.Clear();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}