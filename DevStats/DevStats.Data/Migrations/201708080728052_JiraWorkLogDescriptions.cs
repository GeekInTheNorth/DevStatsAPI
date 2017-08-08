namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JiraWorkLogDescriptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkLogTask", "Description", c => c.String());
            AddColumn("dbo.WorkLogStory", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkLogStory", "Description");
            DropColumn("dbo.WorkLogTask", "Description");
        }
    }
}
