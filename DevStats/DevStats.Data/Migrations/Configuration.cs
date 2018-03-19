namespace DevStats.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DevStatContext>
    {
        public Configuration()
        {
            // If actively developing DB changes, set this to false to allow you to manually roll forward/backward.  Set it back to true before merging to master
            AutomaticMigrationsEnabled = true;
            ContextKey = "DevStats.Data.DevStatContext";
        }

        protected override void Seed(DevStatContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
