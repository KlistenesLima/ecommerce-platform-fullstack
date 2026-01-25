using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjetoEcommerce.Application.Categories.DTOs;

namespace ProjetoEcommerce.Application.Categories.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<CategoryResponse>> GetAllAsync();
        Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
        Task<CategoryResponse> UpdateAsync(Guid id, UpdateCategoryRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
