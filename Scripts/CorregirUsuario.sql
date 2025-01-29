-- Verificar si la columna Cargo existe
IF EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.Usuario') AND name = 'Cargo'
)
BEGIN
    -- Si existe, eliminar la columna actual
    ALTER TABLE dbo.Usuario DROP COLUMN Cargo;
END

-- Agregar la columna Cargo con la configuración correcta
ALTER TABLE dbo.Usuario ADD Cargo nvarchar(100) NOT NULL DEFAULT 'Sin asignar';

-- Verificar si la columna Responsabilidades existe
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.Usuario') AND name = 'Responsabilidades'
)
BEGIN
    -- Si no existe, agregarla
    ALTER TABLE dbo.Usuario ADD Responsabilidades nvarchar(500) NULL;
END 