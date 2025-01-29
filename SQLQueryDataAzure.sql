-- Creamos la base de datos si no existe (esto debe ejecutarse conectado a master)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'IsomanagerDB')
BEGIN
    CREATE DATABASE [IsomanagerDB]
END
GO

-- A partir de aquí, debes estar conectado a IsomanagerDB
-- Si las tablas ya existen, se eliminan para evitar conflictos
IF OBJECT_ID('dbo.MejoraProceso', 'U') IS NOT NULL DROP TABLE dbo.MejoraProceso;
IF OBJECT_ID('dbo.KPI', 'U') IS NOT NULL DROP TABLE dbo.KPI;
IF OBJECT_ID('dbo.Foda', 'U') IS NOT NULL DROP TABLE dbo.Foda;
IF OBJECT_ID('dbo.FactoresExternos', 'U') IS NOT NULL DROP TABLE dbo.FactoresExternos;
IF OBJECT_ID('dbo.EvaluacionProceso', 'U') IS NOT NULL DROP TABLE dbo.EvaluacionProceso;
IF OBJECT_ID('dbo.Document', 'U') IS NOT NULL DROP TABLE dbo.Document;
IF OBJECT_ID('dbo.Contexto', 'U') IS NOT NULL DROP TABLE dbo.Contexto;
IF OBJECT_ID('dbo.CambioProceso', 'U') IS NOT NULL DROP TABLE dbo.CambioProceso;
IF OBJECT_ID('dbo.AuditoriaInternaProceso', 'U') IS NOT NULL DROP TABLE dbo.AuditoriaInternaProceso;
IF OBJECT_ID('dbo.AlcanceSistemaGestion', 'U') IS NOT NULL DROP TABLE dbo.AlcanceSistemaGestion;
IF OBJECT_ID('dbo.__MigrationHistory', 'U') IS NOT NULL DROP TABLE dbo.__MigrationHistory;
IF OBJECT_ID('dbo.Usuario', 'U') IS NOT NULL DROP TABLE dbo.Usuario;
IF OBJECT_ID('dbo.Area', 'U') IS NOT NULL DROP TABLE dbo.Area;
IF OBJECT_ID('dbo.Proceso', 'U') IS NOT NULL DROP TABLE dbo.Proceso;
IF OBJECT_ID('dbo.Norma', 'U') IS NOT NULL DROP TABLE dbo.Norma;

-- Tabla Usuario (necesaria para las relaciones)
CREATE TABLE [dbo].[Usuario] (
    [UsuarioId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] [nvarchar](100) NOT NULL,
    [Email] [nvarchar](100) NOT NULL UNIQUE,
    [Password] [nvarchar](100) NOT NULL,
    [Rol] [nvarchar](50) NOT NULL,
    [Cargo] [nvarchar](100) NOT NULL DEFAULT 'Sin asignar',
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Activo',
    [AreaId] [int] NULL,
    [FechaRegistro] [datetime] NOT NULL DEFAULT GETDATE(),
    [UltimaConexion] [datetime] NULL,
    [ContadorIntentos] [int] NOT NULL DEFAULT 0
);
GO

-- Tabla Area
CREATE TABLE [dbo].[Area] (
    [AreaId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] [nvarchar](100) NOT NULL,
    [Descripcion] [nvarchar](500) NULL,
    [Activo] [bit] NOT NULL DEFAULT 1,
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [UltimaModificacion] [datetime] NOT NULL DEFAULT GETDATE()
);
GO

-- Tabla Norma
CREATE TABLE [dbo].[Norma] (
    [NormaId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Titulo] [nvarchar](200) NOT NULL,
    [Descripcion] [nvarchar](500) NULL,
    [Version] [nvarchar](20) NOT NULL,
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Activo',
    [ResponsableId] [int] NOT NULL,
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [UltimaActualizacion] [datetime] NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Norma_Usuario] FOREIGN KEY ([ResponsableId]) REFERENCES [dbo].[Usuario] ([UsuarioId])
);
GO

-- Tabla para el historial de migraciones
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC, [ContextKey] ASC)
);
GO

-- Tabla Contexto
CREATE TABLE [dbo].[Contexto] (
    [ContextoId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Titulo] [nvarchar](200) NOT NULL,
    [Descripcion] [nvarchar](500) NULL,
    [NormaId] [int] NOT NULL,
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [UltimaActualizacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [FK_Contexto_Norma] FOREIGN KEY ([NormaId]) REFERENCES [dbo].[Norma] ([NormaId])
);
GO

-- Tabla AlcanceSistemaGestion
CREATE TABLE [dbo].[AlcanceSistemaGestion] (
    [AlcanceId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Descripcion] [nvarchar](1000) NOT NULL,
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [UltimaModificacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [Activo] [bit] NOT NULL DEFAULT 1,
    [ContextoId] [int] NOT NULL,
    CONSTRAINT [FK_AlcanceSistemaGestion_Contexto] FOREIGN KEY ([ContextoId]) REFERENCES [dbo].[Contexto] ([ContextoId])
);
GO

-- Tabla Proceso
CREATE TABLE [dbo].[Proceso] (
    [ProcesoId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] [nvarchar](100) NOT NULL,
    [Descripcion] [nvarchar](500) NULL,
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Activo',
    [AreaId] [int] NOT NULL,
    [ResponsableId] [int] NOT NULL,
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [UltimaModificacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [Activo] [bit] NOT NULL DEFAULT 1,
    CONSTRAINT [FK_Proceso_Area] FOREIGN KEY ([AreaId]) REFERENCES [dbo].[Area] ([AreaId]),
    CONSTRAINT [FK_Proceso_Usuario] FOREIGN KEY ([ResponsableId]) REFERENCES [dbo].[Usuario] ([UsuarioId])
);
GO

-- Tabla AuditoriaInternaProceso
CREATE TABLE [dbo].[AuditoriaInternaProceso] (
    [AuditoriaId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Titulo] [nvarchar](200) NOT NULL,
    [Descripcion] [nvarchar](500) NOT NULL,
    [Comentarios] [nvarchar](max) NULL,
    [ProcesoId] [int] NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [FechaAuditoria] [datetime] NOT NULL DEFAULT GETDATE(),
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Pendiente',
    CONSTRAINT [FK_AuditoriaInternaProceso_Proceso] FOREIGN KEY ([ProcesoId]) REFERENCES [dbo].[Proceso] ([ProcesoId]),
    CONSTRAINT [FK_AuditoriaInternaProceso_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([UsuarioId])
);
GO

-- Tabla CambioProceso
CREATE TABLE [dbo].[CambioProceso] (
    [CambioId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Titulo] [nvarchar](100) NOT NULL,
    [Descripcion] [nvarchar](500) NOT NULL,
    [ProcesoId] [int] NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [TipoCambio] [nvarchar](50) NOT NULL,
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Pendiente',
    [FechaCambio] [datetime] NOT NULL DEFAULT GETDATE(),
    [Activo] [bit] NOT NULL DEFAULT 1,
    CONSTRAINT [FK_CambioProceso_Proceso] FOREIGN KEY ([ProcesoId]) REFERENCES [dbo].[Proceso] ([ProcesoId]),
    CONSTRAINT [FK_CambioProceso_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([UsuarioId])
);
GO

-- Tabla Document
CREATE TABLE [dbo].[Document] (
    [DocumentId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] [nvarchar](200) NOT NULL,
    [Ruta] [nvarchar](500) NOT NULL,
    [Version] [nvarchar](20) NOT NULL DEFAULT '1.0',
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Activo',
    [UsuarioId] [int] NOT NULL,
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [UltimaModificacion] [datetime] NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Document_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([UsuarioId])
);
GO

-- Tabla EvaluacionProceso
CREATE TABLE [dbo].[EvaluacionProceso] (
    [EvaluacionId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ProcesoId] [int] NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [Fecha] [datetime] NOT NULL DEFAULT GETDATE(),
    [Calificacion] [int] NOT NULL,
    [Observaciones] [nvarchar](max) NULL,
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Pendiente',
    CONSTRAINT [FK_EvaluacionProceso_Proceso] FOREIGN KEY ([ProcesoId]) REFERENCES [dbo].[Proceso] ([ProcesoId]),
    CONSTRAINT [FK_EvaluacionProceso_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([UsuarioId])
);
GO

-- Tabla FactoresExternos
CREATE TABLE [dbo].[FactoresExternos] (
    [FactorId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] [nvarchar](100) NOT NULL,
    [Descripcion] [nvarchar](1000) NOT NULL,
    [ContextoId] [int] NOT NULL,
    [TipoFactor] [nvarchar](50) NOT NULL,
    [Impacto] [nvarchar](50) NOT NULL,
    [Probabilidad] [nvarchar](50) NOT NULL,
    [AccionesSugeridas] [nvarchar](1000) NULL,
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [Activo] [bit] NOT NULL DEFAULT 1,
    CONSTRAINT [FK_FactoresExternos_Contexto] FOREIGN KEY ([ContextoId]) REFERENCES [dbo].[Contexto] ([ContextoId])
);
GO

-- Tabla Foda
CREATE TABLE [dbo].[Foda] (
    [FodaId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ContextoId] [int] NOT NULL,
    [Fortalezas] [nvarchar](max) NULL,
    [Oportunidades] [nvarchar](max) NULL,
    [Debilidades] [nvarchar](max) NULL,
    [Amenazas] [nvarchar](max) NULL,
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [UltimaActualizacion] [datetime] NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Foda_Contexto] FOREIGN KEY ([ContextoId]) REFERENCES [dbo].[Contexto] ([ContextoId])
);
GO

-- Tabla KPI
CREATE TABLE [dbo].[KPI] (
    [KpiId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] [nvarchar](100) NOT NULL,
    [Descripcion] [nvarchar](500) NULL,
    [Valor] [nvarchar](100) NOT NULL,
    [Meta] [nvarchar](100) NOT NULL,
    [FechaMedicion] [datetime] NOT NULL DEFAULT GETDATE(),
    [ProcesoId] [int] NOT NULL,
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Activo',
    CONSTRAINT [FK_KPI_Proceso] FOREIGN KEY ([ProcesoId]) REFERENCES [dbo].[Proceso] ([ProcesoId])
);
GO

-- Tabla MejoraProceso
CREATE TABLE [dbo].[MejoraProceso] (
    [MejoraId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Titulo] [nvarchar](100) NOT NULL,
    [Descripcion] [nvarchar](500) NOT NULL,
    [ProcesoId] [int] NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [Estado] [nvarchar](50) NOT NULL DEFAULT 'Pendiente',
    [Prioridad] [nvarchar](50) NOT NULL DEFAULT 'Media',
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [FechaImplementacion] [datetime] NULL,
    [Activo] [bit] NOT NULL DEFAULT 1,
    CONSTRAINT [FK_MejoraProceso_Proceso] FOREIGN KEY ([ProcesoId]) REFERENCES [dbo].[Proceso] ([ProcesoId]),
    CONSTRAINT [FK_MejoraProceso_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([UsuarioId])
);
GO

-- Insertar usuario administrador inicial
INSERT INTO [dbo].[Usuario] (
    [Nombre],
    [Email],
    [Password],
    [Rol],
    [Estado],
    [FechaRegistro],
    [ContadorIntentos]
)
VALUES (
    'Administrador',
    'admin@isomanager.com',
    'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', -- Hash de 'admin123'
    'Administrador',
    'Activo',
    GETDATE(),
    0
);
GO
