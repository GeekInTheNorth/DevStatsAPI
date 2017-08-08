namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JiraWorkLogs : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WorkLogTask", t => t.WorkLogTaskID, cascadeDelete: true)
                .Index(t => t.WorkLogTaskID);
            
            CreateTable(
                "dbo.WorkLogTask",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkLogStoryID = c.Int(nullable: false),
                        TaskKey = c.String(),
                        Owner = c.String(),
                        Description = c.String(),
                        Activity = c.String(),
                        Complexity = c.String(),
                        EstimateInSeconds = c.Int(nullable: false),
                        ActualTimeInSeconds = c.Int(nullable: false),
                        LastWorkedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WorkLogStory", t => t.WorkLogStoryID, cascadeDelete: true)
                .Index(t => t.WorkLogStoryID);
            
            CreateTable(
                "dbo.WorkLogStory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StoryKey = c.String(),
                        Description = c.String(),
                        TShirtSize = c.String(),
                        Complexity = c.String(),
                        LooseEstimateInHours = c.Int(nullable: false),
                        EstimateInSeconds = c.Int(nullable: false),
                        ActualTimeInSeconds = c.Int(nullable: false),
                        LastWorkedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkLogTask", "WorkLogStoryID", "dbo.WorkLogStory");
            DropForeignKey("dbo.WorkLogEntry", "WorkLogTaskID", "dbo.WorkLogTask");
            DropIndex("dbo.WorkLogTask", new[] { "WorkLogStoryID" });
            DropIndex("dbo.WorkLogEntry", new[] { "WorkLogTaskID" });
            DropTable("dbo.WorkLogStory");
            DropTable("dbo.WorkLogTask");
            DropTable("dbo.WorkLogEntry");
        }
    }
}
