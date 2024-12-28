-- Migrar los datos existentes
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
    END;
GO 