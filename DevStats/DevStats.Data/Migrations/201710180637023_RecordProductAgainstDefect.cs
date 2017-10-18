namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecordProductAgainstDefect : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Defect", "Product", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Defect", "Product");
        }
    }
}
