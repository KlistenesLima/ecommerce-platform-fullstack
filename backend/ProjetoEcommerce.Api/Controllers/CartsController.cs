using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjetoEcommerce.Application.Cart.Services;
using ProjetoEcommerce.Application.Cart.DTOs.Requests;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            try
            {
                var cart = await _cartService.GetByUserAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem([FromBody] AddToCartRequest request)
        {
            try
            {
                var cart = await _cartService.AddItemAsync(request);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{cartId}/items/{productId}")]
        public async Task<IActionResult> RemoveItem(Guid cartId, Guid productId)
        {
            try
            {
                var cart = await _cartService.RemoveItemAsync(cartId, productId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{cartId}/clear")]
        public async Task<IActionResult> Clear(Guid cartId)
        {
            try
            {
                var cart = await _cartService.ClearAsync(cartId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{cartId}/items/{productId}/quantity")]
        public async Task<IActionResult> UpdateQuantity(Guid cartId, Guid productId, [FromBody] dynamic request)
        {
            try
            {
                var cart = await _cartService.UpdateItemQuantityAsync(cartId, productId, request.quantity);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
