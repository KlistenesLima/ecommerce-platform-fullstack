using System;

namespace ProjetoEcommerce.Application.Cart.DTOs.Requests
{
    public class AddToCartRequest
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
