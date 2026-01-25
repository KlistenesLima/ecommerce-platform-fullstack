using ProjetoEcommerce.Domain.Enums;

namespace ProjetoEcommerce.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string TransactionId { get; set; } = string.Empty;
        public string CardLastFour { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public virtual Order Order { get; set; } = null!;

        // Construtor padrão
        public Payment() { }

        // Construtor com parâmetros
        public Payment(Guid orderId, decimal amount, PaymentMethod method)
        {
            OrderId = orderId;
            Amount = amount;
            PaymentMethod = method;
        }

        // Método para atualizar status
        public void UpdateStatus(PaymentStatus newStatus)
        {
            Status = newStatus;
        }

        // Método para completar pagamento
        public void CompletePayment(string transactionId, string cardLastFour = "")
        {
            TransactionId = transactionId;
            CardLastFour = cardLastFour;
            Status = PaymentStatus.Completed;
            PaymentDate = DateTime.UtcNow;
        }
    }
}