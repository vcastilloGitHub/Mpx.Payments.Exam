using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mpx.Payments.Api.Domain;
using Mpx.Payments.Api.Domain.DTOs;
using Mpx.Payments.Api.Exercises.Parte2;
using Mpx.Payments.Api.Exercises.Parte2.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Mpx.Payments.Api.Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IPagoServiceNew _pagoService;

        public PagosController(IPagoServiceNew pagoService)
        {
            _pagoService = pagoService;
        }

        public async Task<IActionResult> RegistrarPago(
            [FromHeader(Name = "Idempotency-Key")][Required] string idempotencyKey,
            [FromBody] RegistrarPagoRequest request)

        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Header faltante o cabecera",
                    Detail = "Es obligatorio idempotenctKey",
                });
            }

            PagoResponse response = await _pagoService.RegistrarPagoAsync(request, idempotencyKey);

            return CreatedAtAction(nameof(ConsultarPago), new { id = response.Id }, response);
        }

        public async Task<IActionResult> ConsultarPago([FromRoute] Guid id)
        {
            PagoResponse? response = await _pagoService.ObtenerPagoPorIdAsync(id);
            if (response == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Pago No encontrado",
                    Detail = $"No se encontro el pago con el ID:'{id}'",
                });
            }
            return Ok(response);
        }

        public async Task<IActionResult> ReservarPago(
            [FromRoute] Guid id,
            [FromBody] ReversarPagoRequest request)
        {
            PagoResponse response = await _pagoService.ReversarPagoAsync(id, request);
            return Ok(response);
        }
    }


}


