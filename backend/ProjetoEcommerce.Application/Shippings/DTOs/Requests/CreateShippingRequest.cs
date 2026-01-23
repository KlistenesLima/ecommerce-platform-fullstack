using System;

namespace ProjetoEcommerce.Application.Shippings.DTOs.Requests
{
    public class CreateShippingRequest
    {
        public Guid OrderId { get; set; }
        public string Address { get; set; } = default!;
        public string City { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public string Provider { get; set; } = default!;
        public decimal Cost { get; set; }
    }
}