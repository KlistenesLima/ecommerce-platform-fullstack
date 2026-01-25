using Xunit;
using Moq;
using FluentAssertions;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Application.Cart.Services;
using ProjetoEcommerce.Application.Cart.DTOs.Requests;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ProjetoEcommerce.Tests.Application
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _mockCartRepository = new Mock<ICartRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _cartService = new CartService(_mockCartRepository.Object, _mockProductRepository.Object);
        }

        [Fact]
        public async Task GetByUserAsync_WithValidUserId_ReturnsCart()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var cart = new CartEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };

            _mockCartRepository.Setup(x => x.GetByUserAsync(userId))
                .ReturnsAsync(cart);

            // Act
            var result = await _cartService.GetByUserAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task AddItemAsync_WithValidRequest_AddsItemToCart()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new CartEntity
            {
                Id = cartId,
                Items = new List<CartItem>()
            };

            var product = new Product
            {
                Id = productId,
                Price = 50m,
                Stock = 10
            };

            var request = new AddToCartRequest
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = 2
            };

            _mockCartRepository.Setup(x => x.GetByIdAsync(cartId))
                .ReturnsAsync(cart);
            _mockProductRepository.Setup(x => x.GetByIdAsync(productId))
                .ReturnsAsync(product);
            _mockCartRepository.Setup(x => x.UpdateAsync(It.IsAny<CartEntity>()))
                .ReturnsAsync((CartEntity c) => c);

            // Act
            var result = await _cartService.AddItemAsync(request);

            // Assert
            result.Should().NotBeNull();
            _mockCartRepository.Verify(x => x.UpdateAsync(It.IsAny<CartEntity>()), Times.Once);
        }

        [Fact]
        public async Task RemoveItemAsync_WithValidItem_RemovesFromCart()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new CartEntity
            {
                Id = cartId,
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = productId, Quantity = 2, UnitPrice = 50m }
                }
            };

            _mockCartRepository.Setup(x => x.GetByIdAsync(cartId))
                .ReturnsAsync(cart);
            _mockCartRepository.Setup(x => x.UpdateAsync(It.IsAny<CartEntity>()))
                .ReturnsAsync((CartEntity c) => c);

            // Act
            var result = await _cartService.RemoveItemAsync(cartId, productId);

            // Assert
            result.Should().NotBeNull();
            _mockCartRepository.Verify(x => x.UpdateAsync(It.IsAny<CartEntity>()), Times.Once);
        }
    }
}