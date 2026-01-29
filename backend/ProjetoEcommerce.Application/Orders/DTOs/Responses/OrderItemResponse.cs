using System;
namespace ProjetoEcommerce.Application.Orders.DTOs.Responses
{
    public class OrderItemResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
