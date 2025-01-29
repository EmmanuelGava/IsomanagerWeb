-- Verificar si la columna TipoNorma existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Norma]') AND name = 'TipoNorma')
BEGIN
    -- Agregar la columna TipoNorma
    ALTER TABLE [dbo].[Norma]
    ADD [TipoNorma] [nvarchar](50) NOT NULL DEFAULT 'ISO 9001:2015';
END

-- Verificar si la columna UltimaActualizacion existe
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Norma]') AND name = 'UltimaActualizacion')
BEGIN
    -- Renombrar UltimaActualizacion a UltimaModificacion
    EXEC sp_rename 'dbo.Norma.UltimaActualizacion', 'UltimaModificacion', 'COLUMN';
END

-- Actualizar los registros existentes con valores por defecto
UPDATE [dbo].[Norma]
SET [TipoNorma] = 'ISO 9001:2015'
WHERE [TipoNorma] IS NULL OR [TipoNorma] = ''; 