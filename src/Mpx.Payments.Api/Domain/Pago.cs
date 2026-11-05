namespace Mpx.Payments.Api.Domain;

public class Pago
{
    public Guid Id { get; set; }
    public Guid CuentaId { get; set; }
    public decimal Monto { get; set; }
    public string Referencia { get; set; } = string.Empty;
    public string Proveedor { get; set; } = string.Empty;
    public EstadoPago Estado { get; set; } = EstadoPago.Pendiente;
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaAplicacion { get; set; }
    public DateTime? FechaReversa { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;

    public Cuenta? Cuenta { get; set; }
}
