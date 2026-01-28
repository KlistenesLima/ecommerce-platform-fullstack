using System;
namespace ProjetoEcommerce.Application.Cart.DTOs.Responses
{
    public class CartItemResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
