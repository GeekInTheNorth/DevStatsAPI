using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DevStats.Data
{
    public class DevStatContext : DbContext
    {
        public DevStatContext() : base("DevStatSQL")
        {
        }

        public DbSet<BurndownDay> BurndownDays { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}