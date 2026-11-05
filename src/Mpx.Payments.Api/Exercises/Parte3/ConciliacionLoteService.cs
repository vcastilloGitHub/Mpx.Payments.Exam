using Mpx.Payments.Api.Domain;

namespace Mpx.Payments.Api.Exercises.Parte3;

/// <summary>Cliente del proveedor externo. Devuelve el monto que el proveedor registra para la referencia.</summary>
public interface IProveedorPagosClient
{
    Task<decimal> ObtenerMontoAsync(string referencia, CancellationToken ct);
}

/// <summary>
/// PARTE 3.2 – Implemente ConciliarLoteAsync:
///  - Concurrencia máxima de 20 solicitudes al proveedor.
///  - Soporte de cancelación.
/// En docs/RESPUESTAS.md indique cómo agregaría reintentos ante errores transitorios y persistencia por bloques.
/// </summary>
public class ConciliacionLoteService
{
    private readonly IProveedorPagosClient _proveedor;

    public ConciliacionLoteService(IProveedorPagosClient proveedor) => _proveedor = proveedor;

    public Task ConciliarLoteAsync(IAsyncEnumerable<Pago> pagos, CancellationToken ct)
    {
        // TODO (Parte 3.2)
        throw new NotImplementedException("Parte 3.2 – pendiente de implementar por el candidato.");
    }
}
