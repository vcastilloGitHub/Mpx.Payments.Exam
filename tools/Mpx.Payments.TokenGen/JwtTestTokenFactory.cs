using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Mpx.Payments.TokenGen;

/// <summary>
/// Genera tokens JWT de PRUEBA firmados con la clave simétrica de appsettings.Development.json.
/// Solo para el examen; no usar en ningún otro entorno.
/// </summary>
public static class JwtTestTokenFactory
{
    public const string Issuer = "mpx-exam";
    public const string Audience = "mpx-payments-api";
    public const string SigningKey = "MPX-EXAMEN-CLAVE-DE-PRUEBA-NO-USAR-EN-PRODUCCION-2026-0123456789";

    public static string Create(string usuario, IEnumerable<string> roles, int minutos = 60)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario),
            new(JwtRegisteredClaimNames.UniqueName, usuario),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(minutos),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
