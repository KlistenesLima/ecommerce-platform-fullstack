using System;
using System.Collections.Generic;

namespace ProjetoEcommerce.Application.Cart.DTOs.Responses
{
    public class CartResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItemResponse> Items { get; set; } = new();
    }
}
