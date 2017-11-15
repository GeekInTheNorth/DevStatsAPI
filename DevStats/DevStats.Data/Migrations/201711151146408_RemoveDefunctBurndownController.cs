namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDefunctBurndownController : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.BurndownDay");
        }
        
        public override void Down()
        {
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
    }
}
