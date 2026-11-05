namespace Mpx.Payments.Api.Domain;

public class Cliente
{
    public Guid Id { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public EstadoCliente Estado { get; set; } = EstadoCliente.Activo;

    public ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
}
