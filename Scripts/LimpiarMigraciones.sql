-- Eliminar registros de la tabla de migraciones relacionados con TipoFactor
DELETE FROM [dbo].[__MigrationHistory]
WHERE [MigrationId] LIKE '%TipoFactor%'
   OR [MigrationId] LIKE '%FactoresExternos%'; 