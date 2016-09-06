namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AllowedOrigin",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Origin = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BurndownDay",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Sprint = c.String(),
                        Date = c.DateTime(nullable: false),
                        WorkRemaining = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BurndownDay");
            DropTable("dbo.AllowedOrigin");
        }
    }
}
