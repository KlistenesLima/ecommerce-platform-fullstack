using System;

namespace ProjetoEcommerce.Application.Orders.DTOs.Responses
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
