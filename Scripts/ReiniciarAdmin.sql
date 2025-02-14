-- Verificar si existe la ubicación principal
DECLARE @UbicacionId int
SELECT TOP 1 @UbicacionId = UbicacionId FROM UbicacionGeografica WHERE Nombre = 'Sede Principal'

IF @UbicacionId IS NULL
BEGIN
    -- Crear ubicación principal
    INSERT INTO UbicacionGeografica (
        Nombre, 
        Direccion, 
        Ciudad, 
        Estado, 
        Pais, 
        FechaCreacion, 
        UltimaModificacion, 
        Activo
    )
    VALUES (
        'Sede Principal',
        'Dirección Principal',
        'Ciudad Principal',
        'Estado Principal',
        'País Principal',
        GETDATE(),
        GETDATE(),
        1
    )
    SET @UbicacionId = SCOPE_IDENTITY()
END

-- Verificar si existe el usuario admin
IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Email = 'admin@isomanager.com')
BEGIN
    -- Verificar si existe el área por defecto
    DECLARE @AreaId int
    SELECT TOP 1 @AreaId = AreaId FROM Area WHERE Nombre = 'Administración'
    
    IF @AreaId IS NULL
    BEGIN
        -- Crear área por defecto si no existe
        INSERT INTO Area (
            Nombre, 
            Descripcion, 
            Activo, 
            FechaCreacion, 
            UltimaModificacion,
            UbicacionId
        )
        VALUES (
            'Administración', 
            'Área administrativa principal', 
            1, 
            GETDATE(), 
            GETDATE(),
            @UbicacionId
        )
        
        SET @AreaId = SCOPE_IDENTITY()
    END
    
    -- Insertar usuario administrador
    INSERT INTO Usuario (
        Nombre,
        Email,
        Password,
        Rol,
        Estado,
        AreaId,
        FechaRegistro,
        ContadorIntentos,
        Cargo
    )
    VALUES (
        'Administrador',
        'admin@isomanager.com',
        -- Hash de 'admin123' usando SHA256
        'uXDlgqDBOCgdvYf8rgY8V+R5jbUJx1H6yiDgcrYUj/8=',
        'Administrador',
        'Activo',
        @AreaId,
        GETDATE(),
        0,
        'Administrador del Sistema'
    )
END
ELSE
BEGIN
    -- Resetear el estado del usuario admin si ya existe
    UPDATE Usuario
    SET Estado = 'Activo',
        ContadorIntentos = 0
    WHERE Email = 'admin@isomanager.com'
END 