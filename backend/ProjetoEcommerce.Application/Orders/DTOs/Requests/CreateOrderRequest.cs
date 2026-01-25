using System;

namespace ProjetoEcommerce.Application.Orders.DTOs.Requests
{
    public class CreateOrderRequest
    {
        public Guid UserId { get; set; }
        public Guid CartId { get; set; }
        public string ShippingAddress { get; set; } = default!;
        public string BillingAddress { get; set; } = default!;
    }
}
