namespace DevStats.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveReleaseQualityReport : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ReleaseQuality");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReleaseQuality",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Product = c.String(),
                        Version = c.String(),
                        Reworks = c.Int(nullable: false),
                        CodeReviewFailures = c.Int(nullable: false),
                        TestPlanReviewFailures = c.Int(nullable: false),
                        BacklogBugsResolved = c.Int(nullable: false),
                        RegressionBugs = c.Int(nullable: false),
                        DeploymentIssues = c.Int(nullable: false),
                        Hotfixes = c.Int(nullable: false),
                        DaysDelayed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
