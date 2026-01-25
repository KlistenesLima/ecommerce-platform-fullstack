using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoEcommerce.Application.Categories.DTOs;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;

namespace ProjetoEcommerce.Application.Categories.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse?> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return MapToResponse(category);
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(MapToResponse);
        }

        public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Nome da categoria é obrigatório");

            var category = new Category
            {
                Name = request.Name.Trim(),
                Description = request.Description?.Trim() ?? string.Empty
            };

            var created = await _categoryRepository.AddAsync(category);
            return MapToResponse(created);
        }

        public async Task<CategoryResponse> UpdateAsync(Guid id, UpdateCategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Categoria não encontrada");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Nome da categoria é obrigatório");

            category.Name = request.Name.Trim();
            category.Description = request.Description?.Trim() ?? string.Empty;

            var updated = await _categoryRepository.UpdateAsync(category);
            return MapToResponse(updated);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            if (category.Products?.Any() == true)
                throw new InvalidOperationException("Não é possível excluir categoria com produtos vinculados");

            return await _categoryRepository.DeleteAsync(id);
        }

        private static CategoryResponse MapToResponse(Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                ProductsCount = category.Products?.Count ?? 0
            };
        }
    }
}
