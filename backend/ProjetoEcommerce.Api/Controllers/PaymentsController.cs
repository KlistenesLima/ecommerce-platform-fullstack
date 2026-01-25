using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Application.Payments.DTOs.Requests;
using ProjetoEcommerce.Application.Payments.Services;
using System;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService) => _paymentService = paymentService;

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentRequest request)
        {
            try
            {
                var payment = await _paymentService.ProcessPaymentAsync(request);
                return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(Guid id)
        {
            var payment = await _paymentService.GetPaymentAsync(id);
            return payment == null ? NotFound() : Ok(payment);
        }
    }
}
