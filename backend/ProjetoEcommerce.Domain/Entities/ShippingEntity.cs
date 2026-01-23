using ProjetoEcommerce.Domain.Enums;

namespace ProjetoEcommerce.Domain.Entities
{
    public class ShippingEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public ShippingProvider Provider { get; set; }
        public ShippingStatus Status { get; set; } = ShippingStatus.Pending;
        public string TrackingNumber { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public virtual Order Order { get; set; } = null!;

        public ShippingEntity() { }

        public ShippingEntity(Guid orderId, string address, ShippingProvider provider)
        {
            OrderId = orderId;
            Address = address;
            Provider = provider;
        }

        public void UpdateStatus(ShippingStatus newStatus)
        {
            Status = newStatus;
            if (newStatus == ShippingStatus.Delivered)
                DeliveredDate = DateTime.UtcNow;
        }

        public void SetTrackingNumber(string trackingNumber)
        {
            TrackingNumber = trackingNumber;
            if (Status == ShippingStatus.Pending)
                Status = ShippingStatus.Shipped;
            ShippedDate = DateTime.UtcNow;
        }
    }
}