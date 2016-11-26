namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCustomCors : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AllowedOrigin");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AllowedOrigin",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Origin = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
