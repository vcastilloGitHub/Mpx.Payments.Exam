/*
  Datos de volumen OPCIONALES para las Partes 2.2 y 2.3 (reporte y consulta por proveedor).
  Inserta 5,000 pagos ficticios distribuidos en ~45 días. Puede ejecutarse varias veces:
  primero elimina los registros generados por este script (IdempotencyKey 'seed-%').

  Ejecutar después de preparar-entorno, por ejemplo:
    docker exec -i mpx-exam-sql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Mpx_Exam_Local_2026!" -C -d MpxPaymentsExam < tools/datos-volumen.sql
*/
SET NOCOUNT ON;

DELETE FROM dbo.Pagos WHERE IdempotencyKey LIKE 'seed-%';

WITH n AS (
    SELECT TOP (5000) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS i
    FROM sys.all_objects a CROSS JOIN sys.all_objects b
)
INSERT INTO dbo.Pagos (Id, CuentaId, Monto, Referencia, Proveedor, Estado, FechaCreacion, FechaAplicacion, FechaReversa, IdempotencyKey)
SELECT
    NEWID(),
    CASE n.i % 4
        WHEN 0 THEN CAST('AAAAAAAA-0000-0000-0000-000000000001' AS UNIQUEIDENTIFIER)
        WHEN 1 THEN CAST('AAAAAAAA-0000-0000-0000-000000000004' AS UNIQUEIDENTIFIER)
        WHEN 2 THEN CAST('AAAAAAAA-0000-0000-0000-000000000002' AS UNIQUEIDENTIFIER)
        ELSE        CAST('AAAAAAAA-0000-0000-0000-000000000005' AS UNIQUEIDENTIFIER)
    END,
    CAST(100 + (ABS(CHECKSUM(NEWID())) % 900000) / 100.0 AS DECIMAL(18,2)),
    CONCAT('REF-', RIGHT(CONCAT('000000', n.i), 6)),
    -- TELCO-B queda con pocos pagos para que el filtro HAVING COUNT(*) > 100 tenga efecto.
    CASE WHEN n.i % 50 = 0 THEN 'TELCO-B'
         ELSE CASE n.i % 4 WHEN 0 THEN 'LUZ-NORTE' WHEN 1 THEN 'LUZ-SUR' WHEN 2 THEN 'AGUA-CAPITAL' ELSE 'TELCO-A' END
    END,
    x.Estado,
    x.Fecha,
    CASE WHEN x.Estado IN ('Aplicado', 'Reversado') THEN x.Fecha END,
    CASE WHEN x.Estado = 'Reversado' THEN DATEADD(HOUR, 2, x.Fecha) END,
    CONCAT('seed-', n.i)
FROM n
CROSS APPLY (
    SELECT
        CAST(DATEADD(MINUTE, -(n.i * 13), SYSUTCDATETIME()) AS DATETIME2(3)) AS Fecha,
        CASE WHEN n.i % 10 = 0 THEN 'Rechazado'
             WHEN n.i % 25 = 0 THEN 'Reversado'
             ELSE 'Aplicado' END AS Estado
) x;

SELECT COUNT(*) AS PagosGenerados FROM dbo.Pagos WHERE IdempotencyKey LIKE 'seed-%';
