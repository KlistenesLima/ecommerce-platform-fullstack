using AutoMapper;
using ProjetoEcommerce.Application.Payments.DTOs.Requests;
using ProjetoEcommerce.Application.Payments.DTOs.Responses;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Enums;
using ProjetoEcommerce.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Payments.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(CreatePaymentRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null) throw new InvalidOperationException("Pedido não encontrado");
            if (request.Amount != order.TotalAmount)
                throw new InvalidOperationException("Valor inválido");

            var payment = new Payment(request.OrderId, request.Amount, request.Method);
            var transactionId = Guid.NewGuid().ToString("N").Substring(0, 16);
            payment.CompletePayment(transactionId);

            var createdPayment = await _paymentRepository.AddAsync(payment);
            order.UpdateStatus(OrderStatus.Confirmed);
            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<PaymentResponse>(createdPayment);
        }

        public async Task<PaymentResponse> RefundPaymentAsync(Guid paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) throw new InvalidOperationException("Pagamento não encontrado");
            if (payment.Status != PaymentStatus.Completed)
                throw new InvalidOperationException("Apenas pagamentos confirmados podem ser devolvidos");

            payment.UpdateStatus(PaymentStatus.Refunded);
            await _paymentRepository.UpdateAsync(payment);

            var order = await _orderRepository.GetByIdAsync(payment.OrderId);
            order.UpdateStatus(OrderStatus.Cancelled);
            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<PaymentResponse> GetPaymentAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment == null ? null : _mapper.Map<PaymentResponse>(payment);
        }
    }
}
