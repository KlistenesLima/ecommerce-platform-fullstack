using ProjetoEcommerce.Domain.Enums;

namespace ProjetoEcommerce.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid? CartId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string BillingAddress { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public Guid? PaymentId { get; set; }
        public Guid? ShippingId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Payment? Payment { get; set; }
        public virtual ShippingEntity? Shipping { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Order() { }

        public Order(Guid userId, Guid cartId, decimal totalAmount)
        {
            UserId = userId;
            CartId = cartId;
            TotalAmount = totalAmount;
        }

        public Order(Guid userId, string shippingAddress, string billingAddress)
        {
            UserId = userId;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            if (newStatus == OrderStatus.Shipped)
                ShippedDate = DateTime.UtcNow;
            else if (newStatus == OrderStatus.Delivered)
                DeliveredDate = DateTime.UtcNow;
        }
    }
}