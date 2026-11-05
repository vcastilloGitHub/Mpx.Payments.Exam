namespace Mpx.Payments.TokenGen;

// Uso:
//   dotnet run --project tools/Mpx.Payments.TokenGen -- --usuario cajero01
//   dotnet run --project tools/Mpx.Payments.TokenGen -- --usuario super01 --rol Supervisor
//   dotnet run --project tools/Mpx.Payments.TokenGen -- --usuario x --rol Cajero --rol Supervisor --minutos 120

internal static class Cli
{
    public static int Main(string[] args)
    {
        var usuario = "cajero01";
        var roles = new List<string>();
        var minutos = 60;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--usuario" when i + 1 < args.Length:
                    usuario = args[++i];
                    break;
                case "--rol" when i + 1 < args.Length:
                    roles.Add(args[++i]);
                    break;
                case "--minutos" when i + 1 < args.Length && int.TryParse(args[i + 1], out var m):
                    minutos = m;
                    i++;
                    break;
                case "--help":
                case "-h":
                    Console.WriteLine("Opciones: --usuario <nombre> --rol <rol> (repetible) --minutos <n>");
                    return 0;
            }
        }

        if (roles.Count == 0)
        {
            roles.Add("Cajero");
        }

        var token = JwtTestTokenFactory.Create(usuario, roles, minutos);

        Console.WriteLine($"Usuario: {usuario} | Roles: {string.Join(", ", roles)} | Vigencia: {minutos} min");
        Console.WriteLine();
        Console.WriteLine(token);
        Console.WriteLine();
        Console.WriteLine("Header: Authorization: Bearer <token>");
        return 0;
    }
}
