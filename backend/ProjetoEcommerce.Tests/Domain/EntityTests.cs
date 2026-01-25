using Xunit;
using FluentAssertions;
using ProjetoEcommerce.Domain.Entities;
using System;

namespace ProjetoEcommerce.Tests.Domain
{
    public class UserEntityTests
    {
        [Fact]
        public void UserEntity_WhenCreated_ShouldHaveValidProperties()
        {
            // Arrange & Act
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "11999999999",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // Assert
            user.Id.Should().NotBeEmpty();
            user.Email.Should().Be("test@example.com");
            user.FirstName.Should().Be("John");
            user.IsActive.Should().BeTrue();
        }

        [Fact]
        public void UserEntity_WhenCreated_ShouldHaveCollections()
        {
            // Arrange & Act
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            // Assert
            user.Orders.Should().NotBeNull();
            user.Cart.Should().NotBeNull();
            user.Orders.Should().BeEmpty();
            //user.Cart.Should().BeEmpty();
        }
    }

    public class ProductEntityTests
    {
        [Fact]
        public void ProductEntity_WhenCreated_ShouldHaveValidProperties()
        {
            // Arrange & Act
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                StockQuantity = 10,
                Sku = "TEST-001",
                IsActive = true
            };

            // Assert
            product.Id.Should().NotBeEmpty();
            product.Name.Should().Be("Test Product");
            product.Price.Should().Be(99.99m);
            product.StockQuantity.Should().Be(10);
        }

        [Fact]
        public void ProductEntity_WithZeroPrice_ShouldBeValid()
        {
            // Arrange & Act
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Free Product",
                Price = 0m
            };

            // Assert
            product.Price.Should().Be(0);
        }
    }

    public class OrderEntityTests
    {
        [Fact]
        public void OrderEntity_WhenCreated_ShouldHaveValidStatus()
        {
            // Arrange & Act
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                TotalAmount = 100m,
                ShippingAddress = "123 Main St"
            };

            // Assert
            order.Id.Should().NotBeEmpty();
            order.Status.Should().Be(ProjetoEcommerce.Domain.Enums.OrderStatus.Pending);
            order.Items.Should().BeEmpty();
        }
    }
}
