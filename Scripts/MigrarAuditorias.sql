-- 1. Eliminar las columnas si existen
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuditoriasInternaProceso]') AND name = 'Alcance')
BEGIN
    ALTER TABLE [dbo].[AuditoriasInternaProceso]
    DROP COLUMN Alcance;
END

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuditoriasInternaProceso]') AND name = 'Hallazgos')
BEGIN
    ALTER TABLE [dbo].[AuditoriasInternaProceso]
    DROP COLUMN Hallazgos;
END

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuditoriasInternaProceso]') AND name = 'Recomendaciones')
BEGIN
    ALTER TABLE [dbo].[AuditoriasInternaProceso]
    DROP COLUMN Recomendaciones;
END

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuditoriasInternaProceso]') AND name = 'FechaAuditoria')
BEGIN
    ALTER TABLE [dbo].[AuditoriasInternaProceso]
    DROP COLUMN FechaAuditoria;
END
GO

-- 2. Agregar las nuevas columnas
ALTER TABLE [dbo].[AuditoriasInternaProceso]
ADD Alcance nvarchar(500) NULL,
    Hallazgos nvarchar(500) NULL,
    Recomendaciones nvarchar(500) NULL,
    FechaAuditoria datetime NULL;
GO

-- 3. Migrar los datos existentes
UPDATE [dbo].[AuditoriasInternaProceso]
SET 
    Alcance = CASE 
        WHEN Descripcion IS NULL THEN ''
        WHEN CHARINDEX(N'Hallazgos:', Descripcion) > 0 
        THEN SUBSTRING(Descripcion, 1, CHARINDEX(N'Hallazgos:', Descripcion) - 1)
        ELSE Descripcion 
    END,
    Hallazgos = CASE
        WHEN Descripcion IS NULL OR CHARINDEX(N'Hallazgos:', Descripcion) = 0 THEN ''
        WHEN CHARINDEX(N'Recomendaciones:', Descripcion) > 0 
        THEN SUBSTRING(
            Descripcion, 
            CHARINDEX(N'Hallazgos:', Descripcion) + 10,
            CHARINDEX(N'Recomendaciones:', Descripcion) - CHARINDEX(N'Hallazgos:', Descripcion) - 10
        )
        ELSE SUBSTRING(Descripcion, CHARINDEX(N'Hallazgos:', Descripcion) + 10, LEN(Descripcion))
    END,
    Recomendaciones = CASE
        WHEN Descripcion IS NULL OR CHARINDEX(N'Recomendaciones:', Descripcion) = 0 THEN ''
        ELSE SUBSTRING(Descripcion, CHARINDEX(N'Recomendaciones:', Descripcion) + 16, LEN(Descripcion))
    END,
    FechaAuditoria = FechaCreacion;
GO

-- 4. Actualizar las columnas para que no acepten NULL
ALTER TABLE [dbo].[AuditoriasInternaProceso]
ALTER COLUMN Alcance nvarchar(500) NOT NULL;

ALTER TABLE [dbo].[AuditoriasInternaProceso]
ALTER COLUMN Hallazgos nvarchar(500) NOT NULL;

ALTER TABLE [dbo].[AuditoriasInternaProceso]
ALTER COLUMN Recomendaciones nvarchar(500) NOT NULL;

ALTER TABLE [dbo].[AuditoriasInternaProceso]
ALTER COLUMN FechaAuditoria datetime NOT NULL;
GO 