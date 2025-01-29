-- Crear tabla CompetenciaNorma
CREATE TABLE [dbo].[CompetenciaNorma](
    [CompetenciaId] [int] IDENTITY(1,1) NOT NULL,
    [NormaId] [int] NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [Nombre] [nvarchar](100) NOT NULL,
    [NivelRequerido] [int] NOT NULL,
    [FechaCreacion] [datetime] NOT NULL DEFAULT GETDATE(),
    [UltimaActualizacion] [datetime] NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_CompetenciaNorma] PRIMARY KEY CLUSTERED ([CompetenciaId] ASC),
    CONSTRAINT [FK_CompetenciaNorma_Norma] FOREIGN KEY ([NormaId]) REFERENCES [dbo].[Norma] ([NormaId]),
    CONSTRAINT [FK_CompetenciaNorma_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([UsuarioId])
);
GO

-- Crear índices
CREATE NONCLUSTERED INDEX [IX_CompetenciaNorma_NormaId] ON [dbo].[CompetenciaNorma] ([NormaId] ASC);
CREATE NONCLUSTERED INDEX [IX_CompetenciaNorma_UsuarioId] ON [dbo].[CompetenciaNorma] ([UsuarioId] ASC);
GO 