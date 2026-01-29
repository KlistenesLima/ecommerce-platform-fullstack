using System;
using ProjetoEcommerce.Domain.Enums;

namespace ProjetoEcommerce.Application.Shippings.DTOs.Requests
{
    public class UpdateShippingStatusRequest
    {
        public Guid ShippingId { get; set; }
        public ShippingStatus Status { get; set; }
    }
}
