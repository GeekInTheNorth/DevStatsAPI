namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecordDefectMappings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DefectMapping",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OriginalProduct = c.String(maxLength: 50),
                        OriginalModule = c.String(maxLength: 50),
                        DisplayProduct = c.String(maxLength: 50),
                        DisplayModule = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.DefectMapping");
        }
    }
}
