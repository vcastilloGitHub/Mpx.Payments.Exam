using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Mpx.Payments.Api.Data;
using Mpx.Payments.Api.Domain;
using Mpx.Payments.Api.Domain.DTOs;
using Mpx.Payments.Api.Exercises.Parte2.Interfaces;

namespace Mpx.Payments.Api.Exercises.Parte2.Services
{
    public class PagoServicesNew : IPagoServiceNew
    {
        private readonly AppDbContext _db;

        public PagoServicesNew(AppDbContext db) => _db = db;

        public Task<PagoResponse?> ObtenerPagoPorIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagoResponse> RegistrarPagoAsync(AplicarPagoCommand command, CancellationToken ct = default)
        {
            const int maxRetries = 3;
            int retryCount = 0;

            while (true)
            {
                // incio transaccion
                using var transaction = await _db.Database.BeginTransactionAsync(ct);

                try
                {
                    //valido la idempotencia
                    var pagoExistente = await _db.Pagos.FirstOrDefaultAsync(p => p.IdempotencyKey == command.IdempotencyKey, ct);

                    if (pagoExistente != null)
                    {
                        return pagoExistente;
                    }

                    //verifico cuentas
                    var cuentaOrigen = await _db.Cuentas.FirstOrDefaultAsync(c => c.Id == command.CuentaId, ct);

                    if (cuentaOrigen == null)
                    {
                        throw new DirectoryNotFoundException("la cuenta origen no existe");
                    }

                    var cuentaDestino = await _db.Cuentas.FirstOrDefaultAsync(c => c.Id == command.CuentaId, ct);

                    //Debito
                    cuentaOrigen.Saldo -= command.Monto;

                    //Registro

                    var nuevoPago = new Pago
                    {
                        Id = Guid.NewGuid(),
                        CuentaId = command.CuentaId,
                        Monto = command.Monto,
                        Estado = EstadoPago.Aplicado,
                        IdempotencyKey = command.IdempotencyKey,
                        FechaAplicacion = DateTime.UtcNow
                    };
                    _db.Pagos.Add(nuevoPago);
                    await _db.SaveChangesAsync(ct);
                    await transaction.CommitAsync(ct);

                    return MapToResponse(nuevoPago);
                }

                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync(ct);
                    retryCount++;

                    //Reintentar 3
                    if (retryCount > maxRetries)
                    {
                        throw new NotImplementedException();
                    }

                    //Breve espera

                    await Task.Delay(100 * retryCount, ct);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            }
            
        }

        private PagoResponse MapToResponse(Pago nuevoPago)
        {
            return new PagoResponse
            {
                Id = nuevoPago.Id,
                CuentaDestinoId = nuevoPago.CuentaId,

            }
        }

        public Task<PagoResponse> ReversarPagoAsync(Guid id, ReversarPagoRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
