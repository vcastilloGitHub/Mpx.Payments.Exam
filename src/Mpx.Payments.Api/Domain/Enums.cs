namespace Mpx.Payments.Api.Domain;

public enum EstadoCliente
{
    Activo = 1,
    Bloqueado = 2
}

public enum EstadoPago
{
    Pendiente = 1,
    Aplicado = 2,
    Rechazado = 3,
    Reversado = 4
}
