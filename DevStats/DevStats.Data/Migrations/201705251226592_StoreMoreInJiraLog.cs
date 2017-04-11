namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreMoreInJiraLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JiraLog", "Success", c => c.Boolean(nullable: false));
            AddColumn("dbo.JiraLog", "Action", c => c.String());
            DropColumn("dbo.JiraLog", "SourceDomain");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JiraLog", "SourceDomain", c => c.String());
            DropColumn("dbo.JiraLog", "Action");
            DropColumn("dbo.JiraLog", "Success");
        }
    }
}
