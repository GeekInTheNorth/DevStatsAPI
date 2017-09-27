namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecordReleaseAgainstStoryLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkLogStory", "DeliveredInRelease", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkLogStory", "DeliveredInRelease");
        }
    }
}
