using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DevStats.Data.Entities;

namespace DevStats.Data
{
    public class DevStatContext : DbContext
    {
        public DevStatContext() : base("DevStatSQL")
        {
        }

        public DbSet<AllowedOrigin> AllowedOrigins { get; set; }

        public DbSet<BurndownDay> BurndownDays { get; set; }

        public DbSet<Defect> Defects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}