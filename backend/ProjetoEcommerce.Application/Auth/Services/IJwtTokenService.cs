using ProjetoEcommerce.Domain.Entities;

namespace ProjetoEcommerce.Application.Auth.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
