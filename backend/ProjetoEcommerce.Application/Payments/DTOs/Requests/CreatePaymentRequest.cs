using System;
using ProjetoEcommerce.Domain.Enums;

namespace ProjetoEcommerce.Application.Payments.DTOs.Requests
{
    public class CreatePaymentRequest
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
    }
}
