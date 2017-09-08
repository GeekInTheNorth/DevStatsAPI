namespace DevStats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPasswordReset : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "LoginErrors", c => c.Int(nullable: false));
            AddColumn("dbo.User", "PasswordResetToken", c => c.String());
            AddColumn("dbo.User", "PasswordResetTokenExpiry", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "PasswordResetTokenExpiry");
            DropColumn("dbo.User", "PasswordResetToken");
            DropColumn("dbo.User", "LoginErrors");
        }
    }
}
