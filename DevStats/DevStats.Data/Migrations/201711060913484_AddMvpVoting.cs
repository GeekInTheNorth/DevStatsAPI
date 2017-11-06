namespace DevStats.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddMvpVoting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MvpVote",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VoteeId = c.Int(nullable: false),
                        VoterId = c.Int(nullable: false),
                        VoteDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Authorised = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.User", "DisplayName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "DisplayName");
            DropTable("dbo.MvpVote");
        }
    }
}
