namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefectAnalysisFromAhaAndJira : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Defect", "JiraId", c => c.String());
            AddColumn("dbo.Defect", "AhaId", c => c.String());
            DropColumn("dbo.Defect", "DefectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Defect", "DefectId", c => c.String());
            DropColumn("dbo.Defect", "AhaId");
            DropColumn("dbo.Defect", "JiraId");
        }
    }
}
