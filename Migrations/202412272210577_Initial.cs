namespace IsomanagerWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Area",
                c => new
                    {
                        AreaId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        UbicacionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AreaId)
                .ForeignKey("dbo.UbicacionGeografica", t => t.UbicacionId)
                .Index(t => t.UbicacionId);
            
            CreateTable(
                "dbo.Proceso",
                c => new
                    {
                        ProcesoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Objetivo = c.String(),
                        Descripcion = c.String(maxLength: 500),
                        Estado = c.String(nullable: false, maxLength: 50),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimaActualizacion = c.DateTime(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        NormaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        ResponsableId = c.Int(nullable: false),
                        AdministradorId = c.Int(nullable: false),
                        Progreso = c.Int(nullable: false),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(),
                        Contexto_ContextoId = c.Int(),
                    })
                .PrimaryKey(t => t.ProcesoId)
                .ForeignKey("dbo.Contexto", t => t.Contexto_ContextoId)
                .ForeignKey("dbo.Usuario", t => t.AdministradorId)
                .ForeignKey("dbo.Area", t => t.AreaId)
                .ForeignKey("dbo.Norma", t => t.NormaId)
                .ForeignKey("dbo.Usuario", t => t.ResponsableId)
                .Index(t => t.NormaId)
                .Index(t => t.AreaId)
                .Index(t => t.ResponsableId)
                .Index(t => t.AdministradorId)
                .Index(t => t.Contexto_ContextoId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        UsuarioId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        Rol = c.String(nullable: false, maxLength: 50),
                        Estado = c.String(nullable: false, maxLength: 50),
                        AreaId = c.Int(),
                        FechaRegistro = c.DateTime(nullable: false),
                        UltimaConexion = c.DateTime(),
                        ContadorIntentos = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UsuarioId)
                .ForeignKey("dbo.Area", t => t.AreaId)
                .Index(t => t.Email, unique: true)
                .Index(t => t.AreaId);
            
            CreateTable(
                "dbo.AuditoriasInternaProceso",
                c => new
                    {
                        AuditoriaInternaProcesoId = c.Int(nullable: false, identity: true),
                        ProcesoId = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Descripcion = c.String(maxLength: 500),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimaActualizacion = c.DateTime(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 50),
                        CreadorId = c.Int(nullable: false),
                        AsignadoId = c.Int(nullable: false),
                        Usuario_UsuarioId = c.Int(),
                        Contexto_ContextoId = c.Int(),
                    })
                .PrimaryKey(t => t.AuditoriaInternaProcesoId)
                .ForeignKey("dbo.Usuario", t => t.AsignadoId)
                .ForeignKey("dbo.Usuario", t => t.CreadorId)
                .ForeignKey("dbo.Proceso", t => t.ProcesoId)
                .ForeignKey("dbo.Usuario", t => t.Usuario_UsuarioId)
                .ForeignKey("dbo.Contexto", t => t.Contexto_ContextoId)
                .Index(t => t.ProcesoId)
                .Index(t => t.CreadorId)
                .Index(t => t.AsignadoId)
                .Index(t => t.Usuario_UsuarioId)
                .Index(t => t.Contexto_ContextoId);
            
            CreateTable(
                "dbo.CambiosProceso",
                c => new
                    {
                        CambioId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        Estado = c.String(nullable: false, maxLength: 20),
                        ImpactoEstimado = c.String(nullable: false, maxLength: 20),
                        Justificacion = c.String(nullable: false, maxLength: 500),
                        RiesgosAsociados = c.String(maxLength: 500),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimaModificacion = c.DateTime(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        ProcesoId = c.Int(nullable: false),
                        CreadorId = c.Int(nullable: false),
                        AsignadoId = c.Int(nullable: false),
                        Usuario_UsuarioId = c.Int(),
                        Contexto_ContextoId = c.Int(),
                    })
                .PrimaryKey(t => t.CambioId)
                .ForeignKey("dbo.Usuario", t => t.AsignadoId)
                .ForeignKey("dbo.Usuario", t => t.CreadorId)
                .ForeignKey("dbo.Proceso", t => t.ProcesoId)
                .ForeignKey("dbo.Usuario", t => t.Usuario_UsuarioId)
                .ForeignKey("dbo.Contexto", t => t.Contexto_ContextoId)
                .Index(t => t.ProcesoId)
                .Index(t => t.CreadorId)
                .Index(t => t.AsignadoId)
                .Index(t => t.Usuario_UsuarioId)
                .Index(t => t.Contexto_ContextoId);
            
            CreateTable(
                "dbo.Documentos",
                c => new
                    {
                        DocumentoId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        RutaArchivo = c.String(nullable: false, maxLength: 500),
                        TipoArchivo = c.String(nullable: false, maxLength: 10),
                        Version = c.String(nullable: false, maxLength: 20),
                        FechaCarga = c.DateTime(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                        Seccion = c.String(nullable: false, maxLength: 50),
                        SeccionId = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaModificacion = c.DateTime(),
                        Norma_NormaId = c.Int(),
                    })
                .PrimaryKey(t => t.DocumentoId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .ForeignKey("dbo.Norma", t => t.Norma_NormaId)
                .Index(t => t.UsuarioId)
                .Index(t => t.Norma_NormaId);
            
            CreateTable(
                "dbo.EvaluacionProcesos",
                c => new
                    {
                        EvaluacionId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        FechaEvaluacion = c.DateTime(nullable: false),
                        Calificacion = c.Int(nullable: false),
                        Observaciones = c.String(maxLength: 1000),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimaModificacion = c.DateTime(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        ProcesoId = c.Int(nullable: false),
                        CreadorId = c.Int(nullable: false),
                        AsignadoId = c.Int(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 20),
                        Comentarios = c.String(nullable: false, maxLength: 500),
                        Recomendaciones = c.String(nullable: false, maxLength: 500),
                        Usuario_UsuarioId = c.Int(),
                        Contexto_ContextoId = c.Int(),
                    })
                .PrimaryKey(t => t.EvaluacionId)
                .ForeignKey("dbo.Usuario", t => t.AsignadoId)
                .ForeignKey("dbo.Usuario", t => t.CreadorId)
                .ForeignKey("dbo.Proceso", t => t.ProcesoId)
                .ForeignKey("dbo.Usuario", t => t.Usuario_UsuarioId)
                .ForeignKey("dbo.Contexto", t => t.Contexto_ContextoId)
                .Index(t => t.ProcesoId)
                .Index(t => t.CreadorId)
                .Index(t => t.AsignadoId)
                .Index(t => t.Usuario_UsuarioId)
                .Index(t => t.Contexto_ContextoId);
            
            CreateTable(
                "dbo.MejoraProceso",
                c => new
                    {
                        MejoraId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        Estado = c.String(nullable: false, maxLength: 20),
                        Prioridad = c.String(nullable: false, maxLength: 20),
                        Justificacion = c.String(nullable: false, maxLength: 500),
                        ResultadosEsperados = c.String(maxLength: 500),
                        BeneficiosEsperados = c.String(nullable: false, maxLength: 500),
                        RecursosNecesarios = c.String(nullable: false, maxLength: 500),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimaModificacion = c.DateTime(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        ProcesoId = c.Int(nullable: false),
                        CreadorId = c.Int(nullable: false),
                        AsignadoId = c.Int(nullable: false),
                        Usuario_UsuarioId = c.Int(),
                        Usuario_UsuarioId1 = c.Int(),
                        Usuario_UsuarioId2 = c.Int(),
                    })
                .PrimaryKey(t => t.MejoraId)
                .ForeignKey("dbo.Usuario", t => t.AsignadoId)
                .ForeignKey("dbo.Usuario", t => t.CreadorId)
                .ForeignKey("dbo.Proceso", t => t.ProcesoId)
                .ForeignKey("dbo.Usuario", t => t.Usuario_UsuarioId)
                .ForeignKey("dbo.Usuario", t => t.Usuario_UsuarioId1)
                .ForeignKey("dbo.Usuario", t => t.Usuario_UsuarioId2)
                .Index(t => t.ProcesoId)
                .Index(t => t.CreadorId)
                .Index(t => t.AsignadoId)
                .Index(t => t.Usuario_UsuarioId)
                .Index(t => t.Usuario_UsuarioId1)
                .Index(t => t.Usuario_UsuarioId2);
            
            CreateTable(
                "dbo.Norma",
                c => new
                    {
                        NormaId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Descripcion = c.String(maxLength: 500),
                        Version = c.String(nullable: false, maxLength: 20),
                        Estado = c.String(nullable: false, maxLength: 50),
                        ResponsableId = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimaActualizacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NormaId)
                .ForeignKey("dbo.Usuario", t => t.ResponsableId)
                .Index(t => t.ResponsableId);
            
            CreateTable(
                "dbo.Contexto",
                c => new
                    {
                        ContextoId = c.Int(nullable: false, identity: true),
                        NormaId = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Descripcion = c.String(maxLength: 500),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimaActualizacion = c.DateTime(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ContextoId)
                .ForeignKey("dbo.Norma", t => t.NormaId)
                .Index(t => t.NormaId);
            
            CreateTable(
                "dbo.DefinicionObjetivoAlcance",
                c => new
                    {
                        DefinicionObjetivoAlcanceId = c.Int(nullable: false, identity: true),
                        ContextoId = c.Int(nullable: false),
                        Objetivo = c.String(nullable: false, maxLength: 500),
                        Alcance = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.DefinicionObjetivoAlcanceId)
                .ForeignKey("dbo.Contexto", t => t.ContextoId)
                .Index(t => t.ContextoId);
            
            CreateTable(
                "dbo.FactoresExternos",
                c => new
                    {
                        FactorId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        Impacto = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        TipoFactorId = c.Int(nullable: false),
                        ContextoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FactorId)
                .ForeignKey("dbo.Contexto", t => t.ContextoId)
                .ForeignKey("dbo.TipoFactor", t => t.TipoFactorId)
                .Index(t => t.TipoFactorId)
                .Index(t => t.ContextoId);
            
            CreateTable(
                "dbo.TipoFactor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fodas",
                c => new
                    {
                        ContextoId = c.Int(nullable: false),
                        Fortalezas = c.String(maxLength: 4000),
                        Oportunidades = c.String(maxLength: 4000),
                        Debilidades = c.String(maxLength: 4000),
                        Amenazas = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ContextoId)
                .ForeignKey("dbo.Contexto", t => t.ContextoId)
                .Index(t => t.ContextoId);
            
            CreateTable(
                "dbo.UbicacionGeografica",
                c => new
                    {
                        UbicacionId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Ciudad = c.String(nullable: false, maxLength: 100),
                        Estado = c.String(nullable: false, maxLength: 100),
                        Pais = c.String(nullable: false, maxLength: 100),
                        Direccion = c.String(maxLength: 500),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimaModificacion = c.DateTime(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UbicacionId);
            
            CreateTable(
                "dbo.Calificacion",
                c => new
                    {
                        CalificacionId = c.Int(nullable: false, identity: true),
                        UsuarioId = c.Int(nullable: false),
                        EvaluadorId = c.Int(nullable: false),
                        Categoria = c.String(nullable: false, maxLength: 50),
                        Valor = c.Int(nullable: false),
                        Comentarios = c.String(maxLength: 500),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CalificacionId)
                .ForeignKey("dbo.Usuario", t => t.EvaluadorId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.EvaluadorId);
            
            CreateTable(
                "dbo.Desempeno",
                c => new
                    {
                        DesempenoId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        FechaCreacion = c.DateTime(nullable: false),
                        UltimaModificacion = c.DateTime(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DesempenoId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Formacion",
                c => new
                    {
                        FormacionId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        TipoFormacion = c.String(),
                        FechaObtencion = c.DateTime(nullable: false),
                        Duracion = c.Int(nullable: false),
                        Estado = c.String(),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FormacionId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.KPIs",
                c => new
                    {
                        KpiId = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Valor = c.String(nullable: false),
                        FechaMedicion = c.DateTime(nullable: false),
                        ProcesoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KpiId)
                .ForeignKey("dbo.Proceso", t => t.ProcesoId)
                .Index(t => t.ProcesoId);
            
            CreateTable(
                "dbo.Mensaje",
                c => new
                    {
                        MensajeId = c.Int(nullable: false, identity: true),
                        Asunto = c.String(nullable: false, maxLength: 200),
                        Contenido = c.String(nullable: false),
                        FechaEnvio = c.DateTime(nullable: false),
                        RemitenteId = c.Int(nullable: false),
                        DestinatarioId = c.Int(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.MensajeId)
                .ForeignKey("dbo.Usuario", t => t.DestinatarioId)
                .ForeignKey("dbo.Usuario", t => t.RemitenteId)
                .Index(t => t.RemitenteId)
                .Index(t => t.DestinatarioId);
            
            CreateTable(
                "dbo.Tarea",
                c => new
                    {
                        TareaId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Descripcion = c.String(maxLength: 500),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaVencimiento = c.DateTime(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 50),
                        Prioridad = c.String(nullable: false, maxLength: 50),
                        UsuarioAsignadoId = c.Int(nullable: false),
                        UsuarioCreadorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TareaId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioAsignadoId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioCreadorId)
                .Index(t => t.UsuarioAsignadoId)
                .Index(t => t.UsuarioCreadorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tarea", "UsuarioCreadorId", "dbo.Usuario");
            DropForeignKey("dbo.Tarea", "UsuarioAsignadoId", "dbo.Usuario");
            DropForeignKey("dbo.Mensaje", "RemitenteId", "dbo.Usuario");
            DropForeignKey("dbo.Mensaje", "DestinatarioId", "dbo.Usuario");
            DropForeignKey("dbo.KPIs", "ProcesoId", "dbo.Proceso");
            DropForeignKey("dbo.Formacion", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Desempeno", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Calificacion", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Calificacion", "EvaluadorId", "dbo.Usuario");
            DropForeignKey("dbo.Usuario", "AreaId", "dbo.Area");
            DropForeignKey("dbo.Area", "UbicacionId", "dbo.UbicacionGeografica");
            DropForeignKey("dbo.Proceso", "ResponsableId", "dbo.Usuario");
            DropForeignKey("dbo.Proceso", "NormaId", "dbo.Norma");
            DropForeignKey("dbo.Proceso", "AreaId", "dbo.Area");
            DropForeignKey("dbo.Proceso", "AdministradorId", "dbo.Usuario");
            DropForeignKey("dbo.Norma", "ResponsableId", "dbo.Usuario");
            DropForeignKey("dbo.Documentos", "Norma_NormaId", "dbo.Norma");
            DropForeignKey("dbo.Proceso", "Contexto_ContextoId", "dbo.Contexto");
            DropForeignKey("dbo.Contexto", "NormaId", "dbo.Norma");
            DropForeignKey("dbo.Fodas", "ContextoId", "dbo.Contexto");
            DropForeignKey("dbo.FactoresExternos", "TipoFactorId", "dbo.TipoFactor");
            DropForeignKey("dbo.FactoresExternos", "ContextoId", "dbo.Contexto");
            DropForeignKey("dbo.EvaluacionProcesos", "Contexto_ContextoId", "dbo.Contexto");
            DropForeignKey("dbo.DefinicionObjetivoAlcance", "ContextoId", "dbo.Contexto");
            DropForeignKey("dbo.CambiosProceso", "Contexto_ContextoId", "dbo.Contexto");
            DropForeignKey("dbo.AuditoriasInternaProceso", "Contexto_ContextoId", "dbo.Contexto");
            DropForeignKey("dbo.MejoraProceso", "Usuario_UsuarioId2", "dbo.Usuario");
            DropForeignKey("dbo.MejoraProceso", "Usuario_UsuarioId1", "dbo.Usuario");
            DropForeignKey("dbo.MejoraProceso", "Usuario_UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.MejoraProceso", "ProcesoId", "dbo.Proceso");
            DropForeignKey("dbo.MejoraProceso", "CreadorId", "dbo.Usuario");
            DropForeignKey("dbo.MejoraProceso", "AsignadoId", "dbo.Usuario");
            DropForeignKey("dbo.EvaluacionProcesos", "Usuario_UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.EvaluacionProcesos", "ProcesoId", "dbo.Proceso");
            DropForeignKey("dbo.EvaluacionProcesos", "CreadorId", "dbo.Usuario");
            DropForeignKey("dbo.EvaluacionProcesos", "AsignadoId", "dbo.Usuario");
            DropForeignKey("dbo.Documentos", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.CambiosProceso", "Usuario_UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.CambiosProceso", "ProcesoId", "dbo.Proceso");
            DropForeignKey("dbo.CambiosProceso", "CreadorId", "dbo.Usuario");
            DropForeignKey("dbo.CambiosProceso", "AsignadoId", "dbo.Usuario");
            DropForeignKey("dbo.AuditoriasInternaProceso", "Usuario_UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.AuditoriasInternaProceso", "ProcesoId", "dbo.Proceso");
            DropForeignKey("dbo.AuditoriasInternaProceso", "CreadorId", "dbo.Usuario");
            DropForeignKey("dbo.AuditoriasInternaProceso", "AsignadoId", "dbo.Usuario");
            DropIndex("dbo.Tarea", new[] { "UsuarioCreadorId" });
            DropIndex("dbo.Tarea", new[] { "UsuarioAsignadoId" });
            DropIndex("dbo.Mensaje", new[] { "DestinatarioId" });
            DropIndex("dbo.Mensaje", new[] { "RemitenteId" });
            DropIndex("dbo.KPIs", new[] { "ProcesoId" });
            DropIndex("dbo.Formacion", new[] { "UsuarioId" });
            DropIndex("dbo.Desempeno", new[] { "UsuarioId" });
            DropIndex("dbo.Calificacion", new[] { "EvaluadorId" });
            DropIndex("dbo.Calificacion", new[] { "UsuarioId" });
            DropIndex("dbo.Fodas", new[] { "ContextoId" });
            DropIndex("dbo.FactoresExternos", new[] { "ContextoId" });
            DropIndex("dbo.FactoresExternos", new[] { "TipoFactorId" });
            DropIndex("dbo.DefinicionObjetivoAlcance", new[] { "ContextoId" });
            DropIndex("dbo.Contexto", new[] { "NormaId" });
            DropIndex("dbo.Norma", new[] { "ResponsableId" });
            DropIndex("dbo.MejoraProceso", new[] { "Usuario_UsuarioId2" });
            DropIndex("dbo.MejoraProceso", new[] { "Usuario_UsuarioId1" });
            DropIndex("dbo.MejoraProceso", new[] { "Usuario_UsuarioId" });
            DropIndex("dbo.MejoraProceso", new[] { "AsignadoId" });
            DropIndex("dbo.MejoraProceso", new[] { "CreadorId" });
            DropIndex("dbo.MejoraProceso", new[] { "ProcesoId" });
            DropIndex("dbo.EvaluacionProcesos", new[] { "Contexto_ContextoId" });
            DropIndex("dbo.EvaluacionProcesos", new[] { "Usuario_UsuarioId" });
            DropIndex("dbo.EvaluacionProcesos", new[] { "AsignadoId" });
            DropIndex("dbo.EvaluacionProcesos", new[] { "CreadorId" });
            DropIndex("dbo.EvaluacionProcesos", new[] { "ProcesoId" });
            DropIndex("dbo.Documentos", new[] { "Norma_NormaId" });
            DropIndex("dbo.Documentos", new[] { "UsuarioId" });
            DropIndex("dbo.CambiosProceso", new[] { "Contexto_ContextoId" });
            DropIndex("dbo.CambiosProceso", new[] { "Usuario_UsuarioId" });
            DropIndex("dbo.CambiosProceso", new[] { "AsignadoId" });
            DropIndex("dbo.CambiosProceso", new[] { "CreadorId" });
            DropIndex("dbo.CambiosProceso", new[] { "ProcesoId" });
            DropIndex("dbo.AuditoriasInternaProceso", new[] { "Contexto_ContextoId" });
            DropIndex("dbo.AuditoriasInternaProceso", new[] { "Usuario_UsuarioId" });
            DropIndex("dbo.AuditoriasInternaProceso", new[] { "AsignadoId" });
            DropIndex("dbo.AuditoriasInternaProceso", new[] { "CreadorId" });
            DropIndex("dbo.AuditoriasInternaProceso", new[] { "ProcesoId" });
            DropIndex("dbo.Usuario", new[] { "AreaId" });
            DropIndex("dbo.Usuario", new[] { "Email" });
            DropIndex("dbo.Proceso", new[] { "Contexto_ContextoId" });
            DropIndex("dbo.Proceso", new[] { "AdministradorId" });
            DropIndex("dbo.Proceso", new[] { "ResponsableId" });
            DropIndex("dbo.Proceso", new[] { "AreaId" });
            DropIndex("dbo.Proceso", new[] { "NormaId" });
            DropIndex("dbo.Area", new[] { "UbicacionId" });
            DropTable("dbo.Tarea");
            DropTable("dbo.Mensaje");
            DropTable("dbo.KPIs");
            DropTable("dbo.Formacion");
            DropTable("dbo.Desempeno");
            DropTable("dbo.Calificacion");
            DropTable("dbo.UbicacionGeografica");
            DropTable("dbo.Fodas");
            DropTable("dbo.TipoFactor");
            DropTable("dbo.FactoresExternos");
            DropTable("dbo.DefinicionObjetivoAlcance");
            DropTable("dbo.Contexto");
            DropTable("dbo.Norma");
            DropTable("dbo.MejoraProceso");
            DropTable("dbo.EvaluacionProcesos");
            DropTable("dbo.Documentos");
            DropTable("dbo.CambiosProceso");
            DropTable("dbo.AuditoriasInternaProceso");
            DropTable("dbo.Usuario");
            DropTable("dbo.Proceso");
            DropTable("dbo.Area");
        }
    }
}
