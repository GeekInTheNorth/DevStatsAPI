namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefectTracking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Defect",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DefectId = c.String(),
                        Module = c.String(),
                        Type = c.String(),
                        Created = c.DateTime(nullable: false),
                        Closed = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Defect");
        }
    }
}
