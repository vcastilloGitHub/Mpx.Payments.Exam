namespace Mpx.Payments.Api.Domain.DTOs
{
    public class PagoResponse
    {
        public Guid Id { get; set; }
        public string CuentaOrigenId { get; set; }
        public string CuentaDestinoId { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; }
        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaReserva { get; set; }
    }
}
