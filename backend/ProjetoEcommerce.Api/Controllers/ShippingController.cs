using Microsoft.AspNetCore.Mvc;
// Namespace CORRETO (Plural)
using ProjetoEcommerce.Application.Shippings.Services;
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

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateShipping([FromBody] CalculateShippingRequest request)
        {
            var result = await _shippingService.CalculateShippingAsync(request.ZipCode, request.Weight);
            return Ok(new { Cost = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipping(Guid id)
        {
            var result = await _shippingService.GetShippingByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipping([FromBody] CreateShippingRequest request)
        {
            var result = await _shippingService.CreateShippingAsync(request);
            return CreatedAtAction(nameof(GetShipping), new { id = result.Id }, result);
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateShippingStatusRequest request)
        {
            var result = await _shippingService.UpdateStatusAsync(request);
            return Ok(result);
        }
    }

    // DTO auxiliar local se não existir na Application (ou mover para lá se preferir)
    public class CalculateShippingRequest 
    {
        public string ZipCode { get; set; }
        public decimal Weight { get; set; }
    }
}
