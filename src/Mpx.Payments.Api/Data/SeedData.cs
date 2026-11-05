using Microsoft.EntityFrameworkCore;
using Mpx.Payments.Api.Domain;

namespace Mpx.Payments.Api.Data;

/// <summary>
/// Datos semilla ficticios para el examen. Los Id son fijos para que el candidato
/// pueda usarlos directamente en Swagger y en las pruebas.
/// </summary>
public static class SeedData
{
    // Clientes
    public static readonly Guid ClienteActivo1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid ClienteActivo2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid ClienteBloqueado = Guid.Parse("33333333-3333-3333-3333-333333333333");

    // Cuentas
    public static readonly Guid CuentaConSaldo = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");
    public static readonly Guid CuentaSaldoBajo = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002");
    public static readonly Guid CuentaSinSaldo = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000003");
    public static readonly Guid CuentaCliente2 = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000004");
    public static readonly Guid CuentaClienteBloqueado = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000005");

    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>().HasData(
            new { Id = ClienteActivo1, Documento = "001-0000001-1", Nombre = "Ana Pérez (prueba)", Estado = EstadoCliente.Activo },
            new { Id = ClienteActivo2, Documento = "001-0000002-2", Nombre = "Luis Gómez (prueba)", Estado = EstadoCliente.Activo },
            new { Id = ClienteBloqueado, Documento = "001-0000003-3", Nombre = "Carla Ruiz (prueba)", Estado = EstadoCliente.Bloqueado });

        modelBuilder.Entity<Cuenta>().HasData(
            new { Id = CuentaConSaldo, ClienteId = ClienteActivo1, Saldo = 150_000.00m, Moneda = "DOP" },
            new { Id = CuentaSaldoBajo, ClienteId = ClienteActivo1, Saldo = 500.00m, Moneda = "DOP" },
            new { Id = CuentaSinSaldo, ClienteId = ClienteActivo2, Saldo = 0.00m, Moneda = "DOP" },
            new { Id = CuentaCliente2, ClienteId = ClienteActivo2, Saldo = 25_000.00m, Moneda = "DOP" },
            new { Id = CuentaClienteBloqueado, ClienteId = ClienteBloqueado, Saldo = 80_000.00m, Moneda = "DOP" });
    }
}
