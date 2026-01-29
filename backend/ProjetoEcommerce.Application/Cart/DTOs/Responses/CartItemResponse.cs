using System;
using ProjetoEcommerce.Application.Products.DTOs.Responses;

namespace ProjetoEcommerce.Application.Cart.DTOs.Responses
{
    public class CartItemResponse
    {
        public Guid ProductId { get; set; }
        public ProductResponse Product { get; set; } 
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}