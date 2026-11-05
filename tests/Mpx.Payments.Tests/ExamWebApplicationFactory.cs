using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Mpx.Payments.TokenGen;

namespace Mpx.Payments.Tests;

/// <summary>
/// Fábrica base para pruebas de integración (Parte 1.4).
/// Usa el entorno "Development" y la base de datos de docker-compose.
/// El candidato puede sobrescribir ConfigureWebHost para usar Testcontainers o SQLite.
/// </summary>
public class ExamWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
    }

    /// <summary>Crea un HttpClient con un token JWT de prueba para los roles indicados.</summary>
    public HttpClient CreateAuthenticatedClient(string usuario = "cajero01", params string[] roles)
    {
        var client = CreateClient();
        var token = JwtTestTokenFactory.Create(usuario, roles.Length == 0 ? new[] { "Cajero" } : roles);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
