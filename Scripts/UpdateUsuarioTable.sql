-- Verificar si la columna Cargo existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Usuario]') AND name = 'Cargo')
BEGIN
    -- Si no existe, agregarla
    ALTER TABLE [dbo].[Usuario]
    ADD [Cargo] [nvarchar](100) NOT NULL DEFAULT 'Sin asignar'
END
ELSE
BEGIN
    -- Si existe, verificar que tenga el valor por defecto correcto
    DECLARE @DefaultConstraintName nvarchar(200)
    
    -- Obtener el nombre de la restricción por defecto si existe
    SELECT @DefaultConstraintName = Name
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'[dbo].[Usuario]')
    AND col_name(parent_object_id, parent_column_id) = 'Cargo'
    
    -- Si existe una restricción por defecto, eliminarla
    IF @DefaultConstraintName IS NOT NULL
    BEGIN
        EXEC('ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT ' + @DefaultConstraintName)
    END
    
    -- Agregar la restricción por defecto correcta
    ALTER TABLE [dbo].[Usuario]
    ADD CONSTRAINT [DF_Usuario_Cargo] DEFAULT 'Sin asignar' FOR [Cargo]
END

-- Actualizar registros existentes que tengan NULL en Cargo
UPDATE [dbo].[Usuario]
SET [Cargo] = 'Sin asignar'
WHERE [Cargo] IS NULL 