namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecordSprints : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.Sprint");
        }
    }
}
