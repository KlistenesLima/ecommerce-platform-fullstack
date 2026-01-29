using ProjetoEcommerce.Domain.Enums;
using System;
using System.Collections.Generic; // Necessário para ICollection

namespace ProjetoEcommerce.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; private set; }
        
        public UserRole Role { get; set; } 

        // --- PROPRIEDADES DE NAVEGAÇÃO ---
        public virtual Cart Cart { get; set; }
        
        // CORREÇÃO 1: Adicionada a lista de Orders que o DbContext pedia
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Construtor vazio para o EF Core
        protected User() { }

        // CORREÇÃO 2: Construtor que o UserService espera (5 argumentos)
        public User(string firstName, string lastName, string email, string passwordHash, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;
            Role = UserRole.Customer; // Default
        }

        // CORREÇÃO 3: Método UpdateProfile que o UserService chama
        public void UpdateProfile(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void SetAdmin()
        {
            Role = UserRole.Admin;
        }
    }
}
