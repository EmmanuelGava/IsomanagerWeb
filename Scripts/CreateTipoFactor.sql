-- Primero eliminar las claves foráneas que referencian a TipoFactor
DECLARE @SQL nvarchar(MAX)
SELECT @SQL = COALESCE(@SQL + '; ', '') + 'ALTER TABLE [' + OBJECT_SCHEMA_NAME(parent_object_id) + '].[' + OBJECT_NAME(parent_object_id) + '] DROP CONSTRAINT [' + name + ']'
FROM sys.foreign_keys
WHERE referenced_object_id = OBJECT_ID('dbo.TipoFactor')

IF @SQL IS NOT NULL
    EXEC sp_executesql @SQL

-- Eliminar las restricciones de la tabla TipoFactor
DECLARE @ConstraintSQL nvarchar(MAX) = ''
SELECT @ConstraintSQL = COALESCE(@ConstraintSQL + '; ', '') + 'ALTER TABLE [dbo].[TipoFactor] DROP CONSTRAINT [' + name + ']'
FROM sys.objects 
WHERE parent_object_id = OBJECT_ID('dbo.TipoFactor') 
AND type_desc LIKE '%CONSTRAINT'

IF @ConstraintSQL != ''
    EXEC sp_executesql @ConstraintSQL

-- Ahora sí, eliminar la tabla
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipoFactor]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[TipoFactor]
END

-- Crear la tabla
CREATE TABLE [dbo].[TipoFactor](
    [TipoFactorId] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](50) NOT NULL,
    [Descripcion] [nvarchar](200) NULL,
    [Activo] [bit] NOT NULL,
    CONSTRAINT [PK_TipoFactor] PRIMARY KEY CLUSTERED 
    (
        [TipoFactorId] ASC
    )
)

-- Insertar datos iniciales
INSERT INTO [dbo].[TipoFactor] ([Nombre], [Descripcion], [Activo])
VALUES 
    (N'Económico', N'Factores relacionados con la economía y finanzas', 1),
    (N'Social', N'Factores relacionados con la sociedad y cultura', 1),
    (N'Tecnológico', N'Factores relacionados con la tecnología e innovación', 1),
    (N'Legal', N'Factores relacionados con leyes y regulaciones', 1)

-- Insertar tipos de factores internos
INSERT INTO [dbo].[TipoFactor] ([Nombre], [Descripcion], [Categoria], [Activo])
VALUES 
    ('Recursos Humanos', 'Factores relacionados con el personal y la gestión del talento', 'R', 1),
    ('Infraestructura', 'Factores relacionados con instalaciones y equipamiento', 'I', 1),
    ('Procesos', 'Factores relacionados con los procesos internos', 'P', 1),
    ('Finanzas', 'Factores relacionados con recursos financieros', 'F', 1),
    ('Tecnología', 'Factores relacionados con sistemas y tecnología', 'T', 1);

-- Recrear la clave foránea
ALTER TABLE [dbo].[FactoresExternos] 
ADD CONSTRAINT [FK_FactoresExternos_TipoFactor] 
FOREIGN KEY ([TipoFactorId]) REFERENCES [dbo].[TipoFactor] ([TipoFactorId]) 