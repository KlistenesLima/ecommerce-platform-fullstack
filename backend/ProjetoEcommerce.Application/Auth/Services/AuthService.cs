using AutoMapper;
using ProjetoEcommerce.Application.Auth.DTOs.Requests;
using ProjetoEcommerce.Application.Auth.DTOs.Responses;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Enums; // Importante
using ProjetoEcommerce.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IJwtTokenService tokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new Exception("Email ou senha inválidos");
            }

            var response = _mapper.Map<AuthResponse>(user);
            // Aqui passa o objeto User inteiro, que contem o Enum Role
            response.Token = _tokenService.GenerateToken(user);
            
            return response;
        }

        public async Task<AuthResponse> RegisterAsync(CreateUserRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception("Email já cadastrado");
            }

            var user = _mapper.Map<User>(request);
            user.PasswordHash = HashPassword(request.Password);
            
            // Atribuição explícita do Enum
            user.Role = UserRole.Customer; 

            await _userRepository.AddAsync(user);

            var response = _mapper.Map<AuthResponse>(user);
            response.Token = _tokenService.GenerateToken(user);

            return response;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
