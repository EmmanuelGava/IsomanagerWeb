-- Agregar columna NormaId
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Formacion]') AND name = 'NormaId')
BEGIN
    ALTER TABLE [dbo].[Formacion]
    ADD NormaId INT NOT NULL DEFAULT 1;
END

-- Agregar la restricción de clave foránea
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Formacion_Norma]'))
BEGIN
    ALTER TABLE [dbo].[Formacion]
    ADD CONSTRAINT [FK_Formacion_Norma] FOREIGN KEY ([NormaId])
    REFERENCES [dbo].[Normas] ([NormaId]);
END

-- Eliminar el valor predeterminado después de agregar los datos
IF EXISTS (SELECT * FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID(N'[dbo].[Formacion]') AND col_name(parent_object_id, parent_column_id) = 'NormaId')
BEGIN
    DECLARE @DefaultConstraintName nvarchar(200);
    SELECT @DefaultConstraintName = Name FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'[dbo].[Formacion]')
    AND col_name(parent_object_id, parent_column_id) = 'NormaId';
    
    EXEC('ALTER TABLE [dbo].[Formacion] DROP CONSTRAINT ' + @DefaultConstraintName);
END 