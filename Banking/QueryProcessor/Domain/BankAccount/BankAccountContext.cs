using Microsoft.EntityFrameworkCore;

namespace Banking.QueryProcessor.Domain.BankAccount
{
    public class BankAccountContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public BankAccountContext(DbContextOptions<BankAccountContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>().ToTable("BankAccount");
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
        }
    }
}
