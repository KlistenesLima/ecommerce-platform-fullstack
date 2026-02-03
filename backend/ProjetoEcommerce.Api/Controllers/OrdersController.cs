using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Application.Orders.DTOs.Requests;
using ProjetoEcommerce.Application.Orders.Services;
using ProjetoEcommerce.Domain.Enums;
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

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try 
            {
                var userId = GetUserId();
                // O Service usa esse ID real, ignorando qualquer coisa que venha no JSON
                var order = await _orderService.CreateOrderAsync(userId, request);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                // Retorna o erro real para o Frontend ver (ajuda no debug)
                return StatusCode(500, new { Error = ex.Message, Stack = ex.StackTrace });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, request.Status);
            return Ok(order);
        }

        private Guid GetUserId()
        {
            // Tenta achar pelo NameIdentifier (Padrão JWT)
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            
            // Fallback: Tenta achar pelo "sub" ou "id" se o NameIdentifier falhar
            if (claim == null) claim = User.FindFirst("sub");
            if (claim == null) claim = User.FindFirst("id");

            if (claim == null) throw new UnauthorizedAccessException("ID do usuário não encontrado no Token.");
            
            return Guid.Parse(claim.Value);
        }
    }

    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}
