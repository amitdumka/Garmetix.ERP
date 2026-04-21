using Microsoft.EntityFrameworkCore;

namespace Garmetix.Data.Context
{
    public class AppDbContext : DbContext
    {
        private readonly string _databasePath;

        // Add your specific DbSets here as you create your models
        public DbSet<Garmetix.Core.Auth.AppUser> Users { get; set; }

        // public DbSet<Party> Parties { get; set; }
        // public DbSet<Voucher> Vouchers { get; set; }
        // public DbSet<BankAccount> BankAccounts { get; set; }

        public AppDbContext(string databasePath)
        {
            _databasePath = databasePath;
            // Ensures the database file is physically created on the Android/iOS/Windows device
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connect to SQLite
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* * GLOBAL QUERY FILTERS 
             * Once you add your DbSets above, apply the filter here.
             * Example: 
             * modelBuilder.Entity<Party>().HasQueryFilter(p => !p.IsDeleted);
             * modelBuilder.Entity<Voucher>().HasQueryFilter(v => !v.IsDeleted);
             */
        }
    }
}