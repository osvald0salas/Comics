using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comics.Lib
{
    public class ComicContext : DbContext
    {
        public ComicContext() : base("name=default")
        {
            //Database.SetInitializer<ComicContext>(new CreateDatabaseIfNotExists<ComicContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ComicContext, Comics.Lib.Migrations.Configuration>( "default"));
        }

        public DbSet<Comic> Comics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Comic>().Property(p => p.Price).HasPrecision(9, 4);
        }
    }
}
