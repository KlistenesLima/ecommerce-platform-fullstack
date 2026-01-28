using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Application.Cart.DTOs.Requests;
using ProjetoEcommerce.Application.Cart.Services;
using System;
using System.Security.Claims;
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

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            // CORREÇÃO: GetByUserAsync -> GetCartAsync
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null) return NotFound("Carrinho vazio ou não encontrado.");
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddToCartRequest request)
        {
            var userId = GetUserId();
            // CORREÇÃO: AddItemAsync -> AddToCartAsync
            var cart = await _cartService.AddToCartAsync(userId, request);
            return Ok(cart);
        }

        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var userId = GetUserId();
            // CORREÇÃO: RemoveItemAsync -> RemoveFromCartAsync
            await _cartService.RemoveFromCartAsync(userId, productId);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            // CORREÇÃO: ClearAsync -> ClearCartAsync
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpPut("items/quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            var userId = GetUserId();
            // CORREÇÃO: UpdateItemQuantityAsync -> UpdateQuantityAsync
            var cart = await _cartService.UpdateQuantityAsync(userId, request);
            return Ok(cart);
        }

        private Guid GetUserId()
        {
            // Extrai o ID do token JWT
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) throw new UnauthorizedAccessException("Usuário não identificado.");
            return Guid.Parse(claim.Value);
        }
    }
}
