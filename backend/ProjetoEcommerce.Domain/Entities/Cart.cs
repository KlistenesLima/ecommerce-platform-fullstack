using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoEcommerce.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime? ExpiresAt { get; set; }
        
        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        // --- PROPRIEDADE DE NAVEGAÇÃO ---
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        // -------------------------------

        public Cart() { }
        
        public Cart(Guid userId)
        {
            UserId = userId;
            Items = new List<CartItem>();
        }
    }
}
