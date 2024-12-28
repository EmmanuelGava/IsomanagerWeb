-- Eliminar las columnas si existen
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
GO 