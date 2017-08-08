namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JiraWorkLogDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkLogEntry", "Description", c => c.String());
            AddColumn("dbo.WorkLogEntry", "Logged", c => c.DateTime(nullable: false));
            AddColumn("dbo.WorkLogTask", "LastWorkedOn", c => c.DateTime());
            AddColumn("dbo.WorkLogStory", "LastWorkedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkLogStory", "LastWorkedOn");
            DropColumn("dbo.WorkLogTask", "LastWorkedOn");
            DropColumn("dbo.WorkLogEntry", "Logged");
            DropColumn("dbo.WorkLogEntry", "Description");
        }
    }
}
