using Xunit;
using Moq;
using FluentAssertions;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Application.Products.Services;
using ProjetoEcommerce.Application.Products.DTOs.Requests;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ProjetoEcommerce.Tests.Application
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsProductResponse()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                StockQuantity = 10,
                Sku = "TEST-001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _mockProductRepository.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Product");
            result.Price.Should().Be(99.99m);
        }

        [Fact]
        public async Task CreateAsync_WithValidRequest_ReturnsProductResponse()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "New Product",
                Description = "New Product Description",
                Price = 49.99m,
                StockQuantity = 20,
                Sku = "NEW-001",
                CategoryId = Guid.NewGuid(),
                ImageUrl = "https://example.com/image.jpg"
            };

            _mockProductRepository.Setup(x => x.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) => p);

            // Act
            var result = await _productService.CreateAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("New Product");
            result.Price.Should().Be(49.99m);
            _mockProductRepository.Verify(x => x.CreateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Price = 10m },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Price = 20m }
            };

            _mockProductRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockProductRepository.Setup(x => x.DeleteAsync(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _productService.DeleteAsync(productId);

            // Assert
            result.Should().BeTrue();
        }
    }
}
