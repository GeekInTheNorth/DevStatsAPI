namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDefunctSprintData : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Sprint");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Sprint",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Pod = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        DurationDays = c.Int(nullable: false),
                        PlannedEffort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
    }
}
