-- Verificar reglas de firewall a nivel de servidor
SELECT * FROM sys.firewall_rules;

-- Verificar configuración de acceso de Azure
SELECT * FROM sys.database_firewall_rules;

-- Verificar si está habilitado el acceso desde Azure
SELECT CAST(DATABASEPROPERTYEX(DB_NAME(), 'IsAzureServicesEnabled') as bit) as IsAzureServicesEnabled; 