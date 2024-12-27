namespace IsomanagerWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveContextoFromProceso : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Proceso", new[] { "ContextoId" });
            RenameColumn(table: "dbo.Proceso", name: "ContextoId", newName: "Contexto_ContextoId");
            AddColumn("dbo.Proceso", "NormaId", c => c.Int(nullable: false));
            AlterColumn("dbo.Proceso", "Contexto_ContextoId", c => c.Int());
            CreateIndex("dbo.Proceso", "NormaId");
            CreateIndex("dbo.Proceso", "Contexto_ContextoId");
            AddForeignKey("dbo.Proceso", "NormaId", "dbo.Norma", "NormaId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Proceso", "NormaId", "dbo.Norma");
            DropIndex("dbo.Proceso", new[] { "Contexto_ContextoId" });
            DropIndex("dbo.Proceso", new[] { "NormaId" });
            AlterColumn("dbo.Proceso", "Contexto_ContextoId", c => c.Int(nullable: false));
            DropColumn("dbo.Proceso", "NormaId");
            RenameColumn(table: "dbo.Proceso", name: "Contexto_ContextoId", newName: "ContextoId");
            CreateIndex("dbo.Proceso", "ContextoId");
        }
    }
}
