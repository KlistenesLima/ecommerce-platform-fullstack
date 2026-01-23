using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Application.Shipping.DTOs.Requests;
using ProjetoEcommerce.Application.Shipping.Services;
using ProjetoEcommerce.Application.Shippings.DTOs.Requests;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;
        public ShippingController(IShippingService shippingService) => _shippingService = shippingService;

        [HttpPost]
        public async Task<IActionResult> CreateShipping([FromBody] CreateShippingRequest request)
        {
            try
            {
                var shipping = await _shippingService.CreateShippingAsync(request);
                return CreatedAtAction(nameof(GetShipping), new { id = shipping.Id }, shipping);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipping(Guid id)
        {
            var shipping = await _shippingService.GetShippingAsync(id);
            return shipping == null ? NotFound() : Ok(shipping);
        }

        [HttpGet("track/{trackingNumber}")]
        public async Task<IActionResult> TrackShipping(string trackingNumber)
        {
            var shipping = await _shippingService.TrackShippingAsync(trackingNumber);
            return shipping == null ? NotFound() : Ok(shipping);
        }
    }
}
