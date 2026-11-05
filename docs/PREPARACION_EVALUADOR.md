# Preparación del entorno – Guía para el evaluador

Completar **al menos 1 día hábil antes** del examen, en cada equipo de evaluación.

## 1. Requisitos del equipo

| Elemento | Requisito |
| --- | --- |
| Hardware | 16 GB RAM (mínimo 8 GB), 20 GB libres en disco, CPU de 4 núcleos |
| Sistema operativo | Windows 10/11 con WSL 2, macOS 13+ o Linux |
| IDE (una opción) | Visual Studio 2022 17.8+ con la carga "Desarrollo de ASP.NET y web", **o** VS Code + extensión C# Dev Kit |
| SDK | .NET 8 (8.0.100 o superior). Si se instala un SDK más reciente, debe mantenerse también el runtime de .NET 8 |
| Contenedores | Docker Desktop (o Docker Engine) con Docker Compose v2.20+ |
| Control de versiones | Git 2.40+ |
| Cliente SQL (opcional) | SQL Server Management Studio o Azure Data Studio |

## 2. Restricciones (coordinar con el Departamento de TI)

- [ ] Deshabilitar GitHub Copilot, IntelliCode con IA y cualquier extensión o asistente de IA en el IDE.
- [ ] Restringir la navegación a `learn.microsoft.com` durante el examen.
- [ ] Bloquear mensajería y correo personal en el equipo.
- [ ] Usar una cuenta de sistema operativo exclusiva para exámenes, sin acceso a repositorios ni datos corporativos.

## 3. Preparación (una vez por equipo)

1. Clonar o copiar este repositorio en el equipo.
2. Con conexión a internet, ejecutar desde la raíz:
   - Windows: `.\tools\preparar-entorno.ps1`
   - macOS/Linux: `./tools/preparar-entorno.sh`
3. El script verifica el SDK y Docker, levanta SQL Server, restaura los paquetes NuGet (quedan en caché para trabajar sin internet), genera la migración `InitialCreate`, crea la base de datos con los datos semilla y ejecuta las pruebas de humo.
4. **Primera vez:** confirmar en Git la carpeta generada `src/Mpx.Payments.Api/Data/Migrations` en la rama `main`, para que todos los equipos usen la misma migración.
5. Opcional: cargar 5,000 pagos ficticios para las Partes 2.2 y 2.3 con `tools/datos-volumen.sql` (el comando está en el encabezado del script).

## 4. Verificación final (checklist)

- [ ] `dotnet test` muestra 2 pruebas aprobadas.
- [ ] `dotnet run --project src/Mpx.Payments.Api` abre Swagger en `https://localhost:7180/swagger` y `GET /ping` responde 200.
- [ ] `dotnet run --project tools/Mpx.Payments.TokenGen -- --rol Supervisor` imprime un token.
- [ ] El IDE abre `Mpx.Payments.Exam.sln` sin errores y sin extensiones de IA activas.
- [ ] Con el equipo **sin internet**, `dotnet build` sigue funcionando.

## 5. Reinicio entre candidatos

```bash
git checkout main
git branch -D exam/<candidato-anterior>   # después de respaldar su rama
git clean -fdx -e .vs
docker compose down -v
# Volver a ejecutar tools/preparar-entorno (ps1 o sh)
```

Respalde la rama del candidato (por ejemplo, `git bundle create <candidato>.bundle exam/<candidato>`) antes de borrarla y guárdela en el repositorio interno de Recursos Humanos/TI según el procedimiento vigente.

## 6. Problemas frecuentes

| Síntoma | Solución |
| --- | --- |
| `Login failed for user 'sa'` | El contenedor aún inicia; esperar 30 s o revisar `docker compose ps` (estado `healthy`). |
| Puerto 14333 ocupado | Cambiar el puerto en `docker-compose.yml` y en `appsettings.Development.json`. |
| `dotnet ef` no se encuentra | Ejecutar `dotnet tool restore` desde la raíz. |
| Certificado HTTPS no confiable | `dotnet dev-certs https --trust`. |
| Pruebas de humo fallan por base de datos | Confirmar que Docker está activo; las pruebas usan el entorno Development. |

> La contraseña de SQL Server y la clave JWT de este repositorio son exclusivamente de prueba para el contenedor local. No deben reutilizarse en ningún otro entorno. Ante dudas sobre el manejo de datos en los equipos de evaluación, consulte con el Departamento de TI.
