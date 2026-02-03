using System;

namespace ProjetoEcommerce.Domain.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        public OrderCreatedEvent(Guid orderId, Guid userId, string email, string customerName, string phoneNumber, decimal totalAmount)
        {
            OrderId = orderId;
            UserId = userId;
            Email = email;
            CustomerName = customerName;
            PhoneNumber = phoneNumber;
            TotalAmount = totalAmount;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
