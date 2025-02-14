-- Agregar columna Cargo si no existe
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Usuario') AND name = 'Cargo')
BEGIN
    ALTER TABLE dbo.Usuario
    ADD Cargo nvarchar(100) NOT NULL 
        CONSTRAINT DF_Usuario_Cargo DEFAULT 'Sin asignar'
END
GO

-- Actualizar registros existentes que tengan NULL en Cargo
UPDATE dbo.Usuario
SET Cargo = 'Sin asignar'
WHERE Cargo IS NULL 