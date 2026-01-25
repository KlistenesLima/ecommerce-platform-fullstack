using ProjetoEcommerce.Domain.Enums;

namespace ProjetoEcommerce.Application.Orders.DTOs.Requests
{
    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}
