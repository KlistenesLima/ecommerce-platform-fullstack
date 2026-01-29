using System;
using ProjetoEcommerce.Domain.Enums;

namespace ProjetoEcommerce.Application.Shippings.DTOs.Requests
{
    public class CreateShippingRequest
    {
        public Guid OrderId { get; set; }
        public string ZipCode { get; set; }
        public decimal Weight { get; set; }
        public ShippingProvider Provider { get; set; } // Enum
    }
}
