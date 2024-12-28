-- Primero eliminar las claves foráneas
DECLARE @SqlDropFK NVARCHAR(MAX) = ''

SELECT @SqlDropFK = @SqlDropFK + 
    'ALTER TABLE [' + OBJECT_SCHEMA_NAME(fk.parent_object_id) + '].[' + 
    OBJECT_NAME(fk.parent_object_id) + '] DROP CONSTRAINT [' + fk.name + '];'
FROM sys.foreign_keys fk
WHERE OBJECT_NAME(fk.referenced_object_id) IN ('TipoFactor', 'FactoresExternos')

IF @SqlDropFK <> ''
BEGIN
    EXEC(@SqlDropFK)
END

-- Luego eliminar las tablas
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FactoresExternos]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[FactoresExternos]
END

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoFactor]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[TipoFactor]
END 