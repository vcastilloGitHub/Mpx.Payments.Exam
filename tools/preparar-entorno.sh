#!/usr/bin/env bash
# Prepara el entorno del examen práctico MPX (macOS / Linux).
# Ejecutar desde la raíz del repositorio:  ./tools/preparar-entorno.sh
set -euo pipefail
cd "$(dirname "$0")/.."
export ASPNETCORE_ENVIRONMENT=Development

paso() { printf '\n==> %s\n' "$1"; }

paso "Verificando SDK de .NET (8 o superior)"
dotnet --list-sdks | grep -Eq '^(8|9|[1-9][0-9])\.' || { echo "No se encontró el SDK de .NET 8 o superior."; exit 1; }

paso "Verificando Docker"
docker info >/dev/null 2>&1 || { echo "Docker no está en ejecución."; exit 1; }

paso "Levantando SQL Server (docker compose)"
docker compose up -d --wait

paso "Restaurando herramientas y paquetes NuGet"
dotnet tool restore
dotnet restore

paso "Compilando la solución"
dotnet build --no-restore

if [ ! -d "src/Mpx.Payments.Api/Data/Migrations" ]; then
  paso "Generando migración inicial (solo la primera vez)"
  dotnet ef migrations add InitialCreate -p src/Mpx.Payments.Api -s src/Mpx.Payments.Api -o Data/Migrations
fi

paso "Creando / actualizando la base de datos"
dotnet ef database update -p src/Mpx.Payments.Api -s src/Mpx.Payments.Api

paso "Ejecutando pruebas de humo"
dotnet test --no-restore

printf '\nEntorno listo para el examen.\n'
