using Mpx.Payments.Api.Data;
using Mpx.Payments.Api.Domain;

namespace Mpx.Payments.Api.Exercises.Parte3;

#pragma warning disable CS1998 // El código defectuoso es intencional para la Parte 3.1.

/// <summary>
/// PARTE 3.1 – Revisión de código.
/// Este servicio contiene defectos INTENCIONALES. Identifique al menos 6, explique su impacto
/// en docs/RESPUESTAS.md y entregue una versión corregida (puede crear una clase nueva).
/// </summary>
public class ConciliacionService
{
    private static Dictionary<string, decimal> _cache = new();
    private readonly AppDbContext _db;

    public ConciliacionService(AppDbContext db) => _db = db;

    public async void ConciliarAsync(List<Pago> pagos)
    {
        var tareas = pagos.Select(async p =>
        {
            var http = new HttpClient();
            var resp = http.GetStringAsync($"https://proveedor/api/pagos/{p.Referencia}").Result;
            var monto = decimal.Parse(resp);

            if (!_cache.ContainsKey(p.Proveedor))
                _cache.Add(p.Proveedor, 0);
            _cache[p.Proveedor] += monto;

            p.Estado = monto == p.Monto ? EstadoPago.Aplicado : EstadoPago.Rechazado;
            await _db.SaveChangesAsync();
        });

        Task.WaitAll(tareas.ToArray());
    }
}

#pragma warning restore CS1998
