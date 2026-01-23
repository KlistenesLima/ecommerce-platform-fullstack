using ProjetoEcommerce.Application.Payments.DTOs.Requests;
using ProjetoEcommerce.Application.Payments.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Payments.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentAsync(CreatePaymentRequest request);
        Task<PaymentResponse> RefundPaymentAsync(Guid paymentId);
        Task<PaymentResponse> GetPaymentAsync(Guid id);
    }
}
