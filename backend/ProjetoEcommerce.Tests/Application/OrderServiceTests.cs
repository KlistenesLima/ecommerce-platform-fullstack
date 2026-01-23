using Xunit;
using Moq;
using FluentAssertions;
using ProjetoEcommerce.Domain.Entities;
using ProjetoEcommerce.Domain.Enums;
using ProjetoEcommerce.Domain.Interfaces;
using ProjetoEcommerce.Application.Orders.Services;
using ProjetoEcommerce.Application.Orders.DTOs.Requests;
using ProjetoEcommerce.Application.Orders.DTOs.Responses;
using AutoMapper;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ProjetoEcommerce.Tests.Application
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly IMapper _mapper;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockCartRepository = new Mock<ICartRepository>();
            _mockProductRepository = new Mock<IProductRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderResponse>()
                   .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            });
            _mapper = config.CreateMapper();

            _orderService = new OrderService(
                _mockOrderRepository.Object,
                _mockCartRepository.Object,
                _mockProductRepository.Object,
                _mapper
            );
        }

        [Fact]
        public async Task GetOrderAsync_WithValidId_ReturnsOrderResponse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                UserId = Guid.NewGuid(),
                Status = OrderStatus.Pending,
                TotalAmount = 100m,
                ShippingAddress = "123 Main St",
                BillingAddress = "123 Main St",
                OrderDate = DateTime.UtcNow
            };

            _mockOrderRepository.Setup(x => x.GetByIdAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderAsync(orderId);

            // Assert
            result.Should().NotBeNull();
            result!.Status.Should().Be("Pending");
            result.TotalAmount.Should().Be(100m);
        }

        [Fact]
        public async Task CreateOrderAsync_WithEmptyCart_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new CreateOrderRequest
            {
                UserId = userId,
                CartId = Guid.NewGuid(),
                ShippingAddress = "123 Main St",
                BillingAddress = "123 Main St"
            };

            _mockCartRepository.Setup(x => x.GetByIdAsync(request.CartId))
                .ReturnsAsync((CartEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.CreateOrderAsync(userId, request));
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_WithValidStatus_UpdatesOrder()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Status = OrderStatus.Pending
            };

            _mockOrderRepository.Setup(x => x.GetByIdAsync(orderId))
                .ReturnsAsync(order);

            _mockOrderRepository.Setup(x => x.UpdateAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order o) => o);

            // Act
            var result = await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Confirmed);

            // Assert
            result.Should().BeTrue();
            _mockOrderRepository.Verify(x => x.UpdateAsync(It.IsAny<Order>()), Times.Once);
            order.Status.Should().Be(OrderStatus.Confirmed);
        }

        [Fact]
        public async Task CancelOrderAsync_WithValidId_SetsStatusCancelled()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Status = OrderStatus.Pending
            };

            _mockOrderRepository.Setup(x => x.GetByIdAsync(orderId))
                .ReturnsAsync(order);

            _mockOrderRepository.Setup(x => x.UpdateAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order o) => o);

            // Act
            var result = await _orderService.CancelOrderAsync(orderId);

            // Assert
            result.Should().BeTrue();
            order.Status.Should().Be(OrderStatus.Cancelled);
            _mockOrderRepository.Verify(x => x.UpdateAsync(It.IsAny<Order>()), Times.Once);
        }
    }
}
