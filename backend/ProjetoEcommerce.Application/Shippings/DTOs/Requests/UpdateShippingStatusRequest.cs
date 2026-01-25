using ProjetoEcommerce.Domain.Enums;

namespace ProjetoEcommerce.Application.Shipping.DTOs.Requests
{
    public class UpdateShippingStatusRequest
    {
        public ShippingStatus Status { get; set; }
    }
}
