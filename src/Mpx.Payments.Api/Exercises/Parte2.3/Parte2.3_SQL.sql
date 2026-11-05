

SELECT 
   p.Proveedor,
   SUM(p.Monto) AS TotalAplicado,
   COUNT(p.Id) AS CantidadPagos
FROM
   Pagos p
WHERE
   p.Estado = Aplicado
   AND p.FechaCreacion >= DATEADD(month, DATEDIFF(month, 0, GETDATE()),0)
   AND p.FechaCreacion < DATEADD(month, DATEDIFF(month, 0, GETDATE()) + 1, 0)
GROUP BY
   p.Proveedor
HAVING
   COUNT(p.Id) > 100
ORDER BY 
   TotalAplicado DESC;