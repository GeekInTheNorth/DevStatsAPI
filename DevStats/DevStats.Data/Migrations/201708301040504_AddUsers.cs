namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 256),
                        EmailAddress = c.String(),
                        Role = c.String(),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
        }
    }
}
