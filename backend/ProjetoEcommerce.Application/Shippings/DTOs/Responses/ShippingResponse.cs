using System;
namespace ProjetoEcommerce.Application.Shippings.DTOs.Responses
{
    public class ShippingResponse
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string Status { get; set; }
        public string TrackingNumber { get; set; }
        public decimal Cost { get; set; }
        public DateTime EstimatedDelivery { get; set; }
    }
}
