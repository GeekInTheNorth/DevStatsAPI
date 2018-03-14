namespace DevStats.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CorrectApiUrlColumnOnApiLog : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ApiLog", "ApUrl", "ApiUrl");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.ApiLog", "ApiUrl", "ApUrl");
        }
    }
}
