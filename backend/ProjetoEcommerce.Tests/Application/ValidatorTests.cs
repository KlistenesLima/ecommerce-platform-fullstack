using Xunit;
using FluentAssertions;
using ProjetoEcommerce.Application.Validations;
using ProjetoEcommerce.Application.Users.DTOs.Requests;
using ProjetoEcommerce.Application.Products.DTOs.Requests;
using System;

namespace ProjetoEcommerce.Tests.Application
{
    public class ValidatorTests
    {
        [Fact]
        public void CreateUserValidator_WithValidRequest_ShouldNotHaveErrors()
        {
            // Arrange
            var validator = new CreateUserValidator();
            var request = new CreateUserRequest
            {
                Email = "test@example.com",
                Password = "Password123",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "11999999999"
            };

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateUserValidator_WithInvalidEmail_ShouldHaveError()
        {
            // Arrange
            var validator = new CreateUserValidator();
            var request = new CreateUserRequest
            {
                Email = "invalid-email",
                Password = "Password123",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "11999999999"
            };

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        public void CreateUserValidator_WithWeakPassword_ShouldHaveError()
        {
            // Arrange
            var validator = new CreateUserValidator();
            var request = new CreateUserRequest
            {
                Email = "test@example.com",
                Password = "weak",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "11999999999"
            };

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        public void CreateProductValidator_WithValidRequest_ShouldNotHaveErrors()
        {
            // Arrange
            var validator = new CreateProductValidator();
            var request = new CreateProductRequest
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                StockQuantity = 10,
                Sku = "TEST-001",
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateProductValidator_WithZeroPrice_ShouldHaveError()
        {
            // Arrange
            var validator = new CreateProductValidator();
            var request = new CreateProductRequest
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 0,
                StockQuantity = 10,
                Sku = "TEST-001",
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
