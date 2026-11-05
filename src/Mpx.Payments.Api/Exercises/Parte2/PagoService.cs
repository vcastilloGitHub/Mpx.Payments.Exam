using Mpx.Payments.Api.Data;
using Mpx.Payments.Api.Domain;

namespace Mpx.Payments.Api.Exercises.Parte2;

/// <summary>Datos de entrada para aplicar un pago.</summary>
public sealed record AplicarPagoCommand(
    Guid CuentaId,
    decimal Monto,
    string Referencia,
    string Proveedor,
    string IdempotencyKey);

public interface IPagoService
{
    Task<Pago> AplicarPagoAsync(AplicarPagoCommand command, CancellationToken ct = default);
}

/// <summary>
/// PARTE 2.1 – Implemente AplicarPagoAsync.
/// El débito del saldo y la creación del pago deben ser atómicos.
/// Ante DbUpdateConcurrencyException, reintente hasta 3 veces y luego indique conflicto (HTTP 409).
/// Puede cambiar la firma, crear excepciones de dominio y mover esta clase si lo justifica.B
/// </summary>
public class PagoService : IPagoService
{
    private readonly AppDbContext _db;

    public PagoService(AppDbContext db) => _db = db;

    
    public Task<Pago> AplicarPagoAsync(AplicarPagoCommand command, CancellationToken ct = default)
    {
        // TODO (Parte 2.1)
        throw new NotImplementedException("Parte 2.1 – pendiente de implementar por el candidato.");
    }
}
