namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveWorkLogEntry : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkLogEntry", "WorkLogTaskID", "dbo.WorkLogTask");
            DropIndex("dbo.WorkLogEntry", new[] { "WorkLogTaskID" });
            DropTable("dbo.WorkLogEntry");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WorkLogEntry",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkLogTaskID = c.Int(nullable: false),
                        Worker = c.String(),
                        EffortInSeconds = c.Int(nullable: false),
                        Description = c.String(),
                        Logged = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.WorkLogEntry", "WorkLogTaskID");
            AddForeignKey("dbo.WorkLogEntry", "WorkLogTaskID", "dbo.WorkLogTask", "ID", cascadeDelete: true);
        }
    }
}
