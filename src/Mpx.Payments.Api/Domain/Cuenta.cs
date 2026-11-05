namespace Mpx.Payments.Api.Domain;

public class Cuenta
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public decimal Saldo { get; set; }
    public string Moneda { get; set; } = "DOP";

    /// <summary>Token de concurrencia optimista (rowversion en SQL Server).</summary>
    public byte[]? RowVersion { get; set; }

    public Cliente? Cliente { get; set; }
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
