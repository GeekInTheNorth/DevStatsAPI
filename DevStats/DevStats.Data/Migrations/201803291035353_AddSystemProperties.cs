namespace DevStats.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddSystemProperties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemProperty",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        DataType = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemProperty");
        }
    }
}
