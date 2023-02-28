using Microsoft.EntityFrameworkCore;

namespace StoUslugClient.DbClient
{
    public class DbSqLiteContext : DbContext
    {       
        public DbSet<Settings> Settings { get; set; }

        public DbSqLiteContext(DbContextOptions<DbSqLiteContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.ApplyConfiguration(new SettingsEntityConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }
}
