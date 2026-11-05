# Mpx.Payments.Exam – Repositorio base del examen práctico

Repositorio de trabajo para el **Examen Práctico – Desarrollador Senior .NET/C#** de Multipagos Expresos.
Uso interno. Todos los datos son ficticios.

## Antes de empezar

1. Cree su rama: `git checkout -b exam/<su-nombre>`.
2. Verifique que todo funciona: `dotnet test` (deben pasar 2 pruebas de humo).
3. Ejecute la API: `dotnet run --project src/Mpx.Payments.Api` y abra `https://localhost:7180/swagger`.

La base de datos (SQL Server en Docker) ya está creada, migrada y con datos semilla.

## Estructura

```
Mpx.Payments.Exam/
├─ src/Mpx.Payments.Api/
│  ├─ Domain/                 Entidades: Cliente, Cuenta, Pago y enumeraciones
│  ├─ Data/                   AppDbContext, configuraciones Fluent API, datos semilla y migraciones
│  ├─ Exercises/Parte2/       PagoService (2.1) y ReporteService (2.2)
│  ├─ Exercises/Parte3/       ConciliacionService (3.1) y ConciliacionLoteService (3.2)
│  └─ Program.cs              Punto de entrada (marcado con TODO para la Parte 1)
├─ tests/Mpx.Payments.Tests/  xUnit + WebApplicationFactory (agregue aquí sus pruebas)
├─ tools/Mpx.Payments.TokenGen/  Generador de tokens JWT de prueba
├─ docs/RESPUESTAS.md         Plantilla para sus respuestas escritas
└─ docker-compose.yml         SQL Server local
```

Puede reorganizar el código (por ejemplo, en capas) si lo justifica en `docs/RESPUESTAS.md`.

## Datos semilla

| Cliente | Estado | Cuenta (Id) | Saldo (DOP) |
| --- | --- | --- | --- |
| Ana Pérez | Activo | `aaaaaaaa-0000-0000-0000-000000000001` | 150,000.00 |
| Ana Pérez | Activo | `aaaaaaaa-0000-0000-0000-000000000002` | 500.00 |
| Luis Gómez | Activo | `aaaaaaaa-0000-0000-0000-000000000003` | 0.00 |
| Luis Gómez | Activo | `aaaaaaaa-0000-0000-0000-000000000004` | 25,000.00 |
| Carla Ruiz | Bloqueado | `aaaaaaaa-0000-0000-0000-000000000005` | 80,000.00 |

Los Id también están disponibles como constantes en `Data/SeedData.cs`.

## Tokens JWT de prueba

La configuración JWT está en `appsettings.Development.json` (sección `Jwt`). Para generar un token:

```bash
# Rol Cajero (por defecto)
dotnet run --project tools/Mpx.Payments.TokenGen -- --usuario cajero01

# Rol Supervisor (necesario para la reversa)
dotnet run --project tools/Mpx.Payments.TokenGen -- --usuario super01 --rol Supervisor
```

En las pruebas use `ExamWebApplicationFactory.CreateAuthenticatedClient("super01", "Supervisor")`.

## Entregables

- Commits al final de cada parte en su rama.
- Respuestas escritas (Partes 3 y 4) en `docs/RESPUESTAS.md`.
- El código debe compilar. Si no termina algo, deje `// TODO:` explicando qué haría.
