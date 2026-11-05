using Microsoft.EntityFrameworkCore;
using Mpx.Payments.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// Infraestructura base provista por el evaluador
// ---------------------------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Payments")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------------------------------------------------------
// TODO (Parte 1): registre aquí validación, manejo global de errores,
//                 autenticación JWT Bearer (sección "Jwt" de appsettings) y autorización.
// TODO (Partes 2 y 3): registre los servicios de Exercises/ que implemente.
// ---------------------------------------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    if (app.Configuration.GetValue("Database:ApplyMigrationsOnStartup", false))
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
}

// TODO (Parte 1): agregue el middleware que necesite (manejo de errores, autenticación, autorización).

app.MapControllers();

app.MapGet("/ping", () => Results.Ok(new { status = "ok", servicio = "MPX Pagos" }))
   .WithName("Ping");

app.Run();

// Necesario para WebApplicationFactory<Program> en las pruebas de integración.
public partial class Program { }
