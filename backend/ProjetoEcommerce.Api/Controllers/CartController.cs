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
    [Route("api/cart")] // Define a rota fixa para evitar confusão
    [Authorize] // Exige login (Token)
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/cart
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        // POST: api/cart
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] AddToCartRequest request)
        {
            var userId = GetUserId();
            var cart = await _cartService.AddToCartAsync(userId, request);
            return Ok(cart);
        }

        // PUT: api/cart/item
        [HttpPut("item")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            var userId = GetUserId();
            var cart = await _cartService.UpdateQuantityAsync(userId, request);
            return Ok(cart);
        }

        // DELETE: api/cart/{productId}
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var userId = GetUserId();
            await _cartService.RemoveFromCartAsync(userId, productId);
            // Retorna o carrinho atualizado para o front não precisar recarregar tudo
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        // DELETE: api/cart
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        // Método auxiliar para pegar ID do token
        private Guid GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) claim = User.FindFirst("sub");
            if (claim == null) claim = User.FindFirst("id");

            if (claim == null) throw new UnauthorizedAccessException("Usuário não identificado no token.");
            
            return Guid.Parse(claim.Value);
        }
    }
}
