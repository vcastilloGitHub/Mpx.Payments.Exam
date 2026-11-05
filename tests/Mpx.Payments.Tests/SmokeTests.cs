using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Mpx.Payments.TokenGen;

namespace Mpx.Payments.Tests;

/// <summary>
/// Pruebas mínimas que confirman que el entorno funciona antes del examen.
/// Agregue sus pruebas de la Parte 1.4 en un archivo nuevo (por ejemplo, PagosApiTests.cs).
/// </summary>
public class SmokeTests : IClassFixture<ExamWebApplicationFactory>
{
    private readonly ExamWebApplicationFactory _factory;

    public SmokeTests(ExamWebApplicationFactory factory) => _factory = factory;

    [Fact]
    public async Task Ping_responde_200()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/ping");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public void Generador_de_tokens_incluye_rol_y_emisor()
    {
        var token = JwtTestTokenFactory.Create("super01", new[] { "Supervisor" });

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Equal(JwtTestTokenFactory.Issuer, jwt.Issuer);
        Assert.Contains(JwtTestTokenFactory.Audience, jwt.Audiences);
        // El handler serializa ClaimTypes.Role como "role"; JwtBearer lo vuelve a mapear al validar.
        Assert.Contains(jwt.Claims, c => (c.Type == "role" || c.Type == ClaimTypes.Role) && c.Value == "Supervisor");
    }
}
