using Garmetix.Core.Models.Accounting;
using Garmetix.Core.Models.Stores;
using Microsoft.EntityFrameworkCore;

namespace Garmetix.Data.Context
{
    public class AppDbContext : DbContext
    {
        private readonly string _databasePath;

        // Add your specific DbSets here as you create your models

        //Company and Stores 
        public DbSet<Company> Companies { get; set; }
        public DbSet<StoreGroup> StoreGroups { get; set; }
        public DbSet<Store> Stores { get; set; }
        
        public DbSet<Garmetix.Core.Models.Sales.Product> Products { get; set; }
        public DbSet<Garmetix.Core.Models.Sales.Stock> Stocks { get; set; }
        public DbSet<Garmetix.Core.Models.Sales.Invoice> Invoices { get; set; }
        public DbSet<Garmetix.Core.Models.Sales.InvoiceItem> InvoiceItems { get; set; }
        // Authentication DbSets
        public DbSet<Garmetix.Core.Auth.AppUser> Users { get; set; }

        // Accounting DbSets
        public DbSet<Party> Parties { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<LedgerGroup> LedgerGroups { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

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

            // Global Query Filters (Hides deleted data automatically)
            modelBuilder.Entity<Party>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Voucher>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<BankAccount>().HasQueryFilter(x => !x.IsDeleted);
            /* * GLOBAL QUERY FILTERS 
             * Once you add your DbSets above, apply the filter here.
             * Example: 
             * modelBuilder.Entity<Party>().HasQueryFilter(p => !p.IsDeleted);
             * modelBuilder.Entity<Voucher>().HasQueryFilter(v => !v.IsDeleted);
             */
        }
    }
}