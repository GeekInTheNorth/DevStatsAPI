namespace DevStats.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class LogBuildStatusChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BuildStatusLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JiraId = c.String(),
                        BuildNumber = c.String(),
                        CommitSha = c.String(),
                        Status = c.String(),
                        Repository = c.String(),
                        Organisation = c.String(),
                        StatusDate = c.DateTime(nullable: false),
                        BitbucketUpdated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BuildStatusLog");
        }
    }
}
