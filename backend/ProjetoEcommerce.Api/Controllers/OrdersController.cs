using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Application.Orders.DTOs.Requests;
using ProjetoEcommerce.Application.Orders.Services;
using ProjetoEcommerce.Infra.MessageQueue.RabbitMQ;
using ProjetoEcommerce.Api.Workers; // Para ver o novo DTO
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IRabbitMQPublisher _publisher;

        public OrdersController(IOrderService orderService, IRabbitMQPublisher publisher)
        {
            _orderService = orderService;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
                
                // Tenta pegar o email do token (se tiver claim de email) ou usa mock
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "cliente@teste.com";
                var userName = User.Identity?.Name ?? "Cliente VIP";

                var order = await _orderService.CreateOrderAsync(userId, request);

                // === NOTIFICAÇÃO COMPLETA ===
                try 
                {
                    var notification = new NotificationDTO
                    {
                        OrderId = order.Id,
                        CustomerName = userName,
                        Phone = "(11) 99999-9999", 
                        Email = userEmail,
                        Message = $"Seu pedido no valor de {order.TotalAmount:C} foi recebido com sucesso!"
                    };

                    await _publisher.PublishAsync("", "sms_notifications_queue", notification);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"[ERRO FILA] {ex.Message}");
                }

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrderAsync(id);
            return order == null ? NotFound() : Ok(order);
        }
    }
}
