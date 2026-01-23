using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Application.Products.DTOs.Requests;
using ProjetoEcommerce.Application.Products.DTOs.Responses;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoEcommerce.Application.Products.Services
{
    public interface IProductService
    {
        Task<ProductResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductResponse>> GetAllAsync();
        Task<IEnumerable<ProductResponse>> GetByCategoryAsync(Guid categoryId);
        Task<ProductResponse> CreateAsync(CreateProductRequest request);
        Task<ProductResponse> UpdateAsync(Guid id, CreateProductRequest request);
        Task<bool> DeleteAsync(Guid id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Produto não encontrado");

            return MapToResponse(product);
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToResponse);
        }

        public async Task<IEnumerable<ProductResponse>> GetByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products.Select(MapToResponse);
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Sku = request.Sku,
                CategoryId = request.CategoryId,
                ImageUrl = request.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            product.SetStock(request.StockQuantity);
            var createdProduct = await _productRepository.CreateAsync(product);
            return MapToResponse(createdProduct);
        }

        public async Task<ProductResponse> UpdateAsync(Guid id, CreateProductRequest request)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Produto não encontrado");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;

            // 🔥 ÚNICO ponto válido para alterar estoque
            product.SetStock(request.StockQuantity);

            await _productRepository.UpdateAsync(product);
            return MapToResponse(product);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        private ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Sku = product.Sku,
                ImageUrl = product.ImageUrl,
                CreatedAt = product.CreatedAt,
                IsActive = product.IsActive
            };
        }
    }
}
