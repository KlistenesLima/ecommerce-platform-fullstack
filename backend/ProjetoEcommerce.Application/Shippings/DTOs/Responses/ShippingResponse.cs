using System;
namespace ProjetoEcommerce.Application.Shipping.DTOs.Responses
{
    public class ShippingResponse
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Provider { get; set; }
        public string Status { get; set; }
        public string TrackingNumber { get; set; }
        public decimal Cost { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
    }
}
