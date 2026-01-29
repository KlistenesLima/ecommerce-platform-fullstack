using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoEcommerce.Application.Cart.DTOs.Requests
{
    public class UpdateQuantityRequest
    {
        [Required]
        public Guid ProductId { get; set; } // Faltava isso!

        [Required]
        [Range(1, 999)]
        public int Quantity { get; set; }
    }
}
