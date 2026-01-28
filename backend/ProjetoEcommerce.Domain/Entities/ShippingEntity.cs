using ProjetoEcommerce.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEcommerce.Domain.Entities
{
    // Nome da tabela no banco
    [Table("Shippings")]
    public class ShippingEntity : BaseEntity
    {
        public Guid OrderId { get; set; }
        
        // CORREÇÃO: Usar Enum, não string
        public ShippingProvider Provider { get; set; }
        
        public string TrackingNumber { get; set; }
        
        public ShippingStatus Status { get; set; }
        
        public decimal Cost { get; set; }
        
        // CORREÇÃO: O campo que estava faltando
        public DateTime EstimatedDelivery { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
