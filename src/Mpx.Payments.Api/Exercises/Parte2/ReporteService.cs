using Microsoft.EntityFrameworkCore;
using Mpx.Payments.Api.Data;
using Mpx.Payments.Api.Domain;

namespace Mpx.Payments.Api.Exercises.Parte2;

public sealed record ReporteDto(string Cliente, decimal Monto, string Proveedor);

/// <summary>
/// PARTE 2.2 – Este método genera el reporte diario y tarda más de 40 segundos en producción.
/// Identifique los problemas (documéntelos en docs/RESPUESTAS.md) y reescríbalo.
/// </summary>
public class ReporteService
{
    private readonly AppDbContext _db;

    public ReporteService(AppDbContext db) => _db = db;

    //public async Task<List<ReporteDto>> ReporteDiarioAsync(DateTime fecha)
    //{
    //    var pagos = await _db.Pagos.ToListAsync();
    //    var resultado = new List<ReporteDto>();
    //    foreach (var p in pagos.Where(x => x.FechaCreacion.Date == fecha.Date))
    //    {
    //        var cuenta = await _db.Cuentas.FirstAsync(c => c.Id == p.CuentaId);
    //        var cliente = await _db.Clientes.FirstAsync(c => c.Id == cuenta.ClienteId);
    //        resultado.Add(new ReporteDto(cliente.Nombre, p.Monto, p.Proveedor));
    //    }
    //    return resultado.OrderByDescending(r => r.Monto).ToList();
    //}

    public async Task<List<ReporteDto>> ReporteDiarioAsync(DateTime fecha, CancellationToken ct = default)
    {
        var fechaInicio = fecha.Date;
        var fechaFin = fechaInicio.AddDays(1);

        return await _db.Pagos
            .AsNoTracking()
            .Where(p => p.FechaCreacion >= fechaInicio && p.FechaCreacion <= fechaFin) //filtro
            .Join(
                _db.Cuentas.AsNoTracking(),
                pago => pago.CuentaId,
                cuenta => cuenta.Id,
                (pago, cuenta) => new { pago, cuenta }
            )
            .Join(
                _db.Clientes.AsNoTracking(),
                pc => pc.cuenta.ClienteId,
                cliente => cliente.Id,
                (pc, cliente) => new ReporteDto
                {
                    Cliente = cliente.Nombre,
                    Monto = pc.pago.Monto,
                    Proveedor = pc.pago.Proveedor,
                }
            )
            .OrderByDesending(r => r.Monto).ToListAsync(ct);

    }
}
