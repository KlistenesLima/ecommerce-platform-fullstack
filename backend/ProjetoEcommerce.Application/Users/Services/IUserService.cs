using ProjetoEcommerce.Application.Users.DTOs.Requests;
using ProjetoEcommerce.Application.Users.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Users.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUserAsync(Guid id);
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<bool> UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
