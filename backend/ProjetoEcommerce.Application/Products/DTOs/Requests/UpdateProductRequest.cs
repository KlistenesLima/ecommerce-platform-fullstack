using System;

namespace ProjetoEcommerce.Application.Products.DTOs.Requests
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Sku { get; set; }
        public Guid CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
