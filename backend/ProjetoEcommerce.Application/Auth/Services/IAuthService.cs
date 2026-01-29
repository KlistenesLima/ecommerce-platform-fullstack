using ProjetoEcommerce.Application.Auth.DTOs.Requests;
using ProjetoEcommerce.Application.Auth.DTOs.Responses;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Auth.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(CreateUserRequest request);
    }
}
