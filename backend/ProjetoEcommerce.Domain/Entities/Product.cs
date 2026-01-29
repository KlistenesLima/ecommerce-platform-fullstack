using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEcommerce.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        [NotMapped]
        public int StockQuantity{ get => Stock; set => Stock = value;}
        public string Sku { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public Product() { }

        public Product(string name, string description, decimal price, int stock)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity > Stock)
                throw new InvalidOperationException("Quantidade insuficiente em estoque");
            Stock -= quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncreaseStock(int quantity)
        {
            Stock += quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetStock(int quantity)
        {
            Stock = quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Preço não pode ser negativo");
            Price = newPrice;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}