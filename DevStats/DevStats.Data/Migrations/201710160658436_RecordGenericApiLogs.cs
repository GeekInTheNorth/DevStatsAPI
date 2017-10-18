namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecordGenericApiLogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApiName = c.String(),
                        ApUrl = c.String(),
                        IssueKey = c.String(),
                        Triggered = c.DateTime(nullable: false),
                        Success = c.Boolean(nullable: false),
                        Action = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ApiLog");
        }
    }
}
