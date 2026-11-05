using System.ComponentModel.DataAnnotations;

namespace Mpx.Payments.Api.Domain.DTOs
{
    public class RegistrarPagoRequest
    {
        
        public string CuentaOrigenId { get; set; }
        public string CuentaDestinoId { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage ="El Monto debe ser mayor a 0.00")]
        public decimal Monto { get; set; }
        public string Moneda { get; set; } 
    }
}
