namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JiraHook : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JiraHookLog");
        }
    }
}
