<#
.SYNOPSIS
  Prepara el entorno del examen práctico MPX (Windows / PowerShell).
.DESCRIPTION
  Verifica requisitos, levanta SQL Server en Docker, restaura paquetes, genera la migración
  inicial (solo la primera vez), aplica la base de datos y ejecuta las pruebas de humo.
  Ejecutar desde la raíz del repositorio:  .\tools\preparar-entorno.ps1
#>
$ErrorActionPreference = 'Stop'
Set-Location (Join-Path $PSScriptRoot '..')
$env:ASPNETCORE_ENVIRONMENT = 'Development'

function Paso([string]$mensaje) { Write-Host "`n==> $mensaje" -ForegroundColor Cyan }
function Ejecutar([string]$descripcion, [scriptblock]$comando) {
    & $comando
    if ($LASTEXITCODE -ne 0) { throw "Falló: $descripcion (código $LASTEXITCODE)" }
}

Paso 'Verificando SDK de .NET (8 o superior)'
$sdks = dotnet --list-sdks
if (-not ($sdks | Where-Object { $_ -match '^(8|9|[1-9]\d)\.' })) { throw 'No se encontró el SDK de .NET 8 o superior.' }
$sdks | ForEach-Object { Write-Host "    $_" }

Paso 'Verificando Docker'
docker info *> $null
if ($LASTEXITCODE -ne 0) { throw 'Docker no está en ejecución. Inicie Docker Desktop y vuelva a intentar.' }

Paso 'Levantando SQL Server (docker compose)'
Ejecutar 'docker compose up' { docker compose up -d --wait }

Paso 'Restaurando herramientas y paquetes NuGet'
Ejecutar 'dotnet tool restore' { dotnet tool restore }
Ejecutar 'dotnet restore' { dotnet restore }

Paso 'Compilando la solución'
Ejecutar 'dotnet build' { dotnet build --no-restore }

if (-not (Test-Path 'src/Mpx.Payments.Api/Data/Migrations')) {
    Paso 'Generando migración inicial (solo la primera vez)'
    Ejecutar 'dotnet ef migrations add' { dotnet ef migrations add InitialCreate -p src/Mpx.Payments.Api -s src/Mpx.Payments.Api -o Data/Migrations }
}

Paso 'Creando / actualizando la base de datos'
Ejecutar 'dotnet ef database update' { dotnet ef database update -p src/Mpx.Payments.Api -s src/Mpx.Payments.Api }

Paso 'Ejecutando pruebas de humo'
Ejecutar 'dotnet test' { dotnet test --no-restore }

Write-Host "`nEntorno listo para el examen." -ForegroundColor Green
Write-Host 'Opcional: cargue datos de volumen con tools/datos-volumen.sql (ver docs/PREPARACION_EVALUADOR.md).'
