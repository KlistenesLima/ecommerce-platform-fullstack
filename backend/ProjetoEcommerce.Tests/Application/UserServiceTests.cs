using Xunit;
using Moq;
using FluentAssertions;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Application.Users.Services;
using ProjetoEcommerce.Application.Users.DTOs.Requests;
using ProjetoEcommerce.Application.Users.DTOs.Responses;
using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProjetoEcommerce.Tests.Application
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserResponse>();
            });
            _mapper = config.CreateMapper();

            _userService = new UserService(_mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetUserAsync_WithValidId_ReturnsUserResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "11999999999",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be("test@example.com");
            result.FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetUserAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateUserAsync_WithValidRequest_ReturnsUserResponse()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Email = "newuser@example.com",
                Password = "Senha123",
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "11988888888"
            };

            _mockUserRepository.Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync((User)null);

            _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            // Act
            var result = await _userService.CreateUserAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("newuser@example.com");
            result.FirstName.Should().Be("Jane");
            _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_WithExistingEmail_ThrowsException()
        {
            // Arrange
            var request = new CreateUserRequest
            {
                Email = "existing@example.com",
                Password = "Senha123",
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "11988888888"
            };

            var existingUser = new User(request.Email, "hashedPassword", request.FirstName, request.LastName, request.PhoneNumber);

            _mockUserRepository.Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateUserAsync(request));
        }

        [Fact]
        public async Task DeleteUserAsync_WithValidId_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId };

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserRepository.Setup(x => x.DeleteAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            result.Should().BeTrue();
            _mockUserRepository.Verify(x => x.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Email = "user1@example.com", FirstName = "User", LastName = "One" },
                new User { Id = Guid.NewGuid(), Email = "user2@example.com", FirstName = "User", LastName = "Two" }
            };

            _mockUserRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }
    }
}
