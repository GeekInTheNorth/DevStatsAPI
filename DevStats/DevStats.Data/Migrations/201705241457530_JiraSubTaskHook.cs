namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JiraSubTaskHook : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JiraLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IssueIdentity = c.String(),
                        IssueKey = c.String(),
                        SourceDomain = c.String(),
                        Content = c.String(),
                        Triggered = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.JiraHookLog");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.JiraHookLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserIdentity = c.String(),
                        UserKey = c.String(),
                        Body = c.String(),
                        Triggered = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.JiraLog");
        }
    }
}
