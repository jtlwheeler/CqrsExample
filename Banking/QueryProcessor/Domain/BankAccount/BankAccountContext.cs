using Microsoft.EntityFrameworkCore;

namespace Banking.QueryProcessor.Domain.BankAccount
{
    public class BankAccountContext : DbContext
    {
        public BankAccountContext(DbContextOptions<BankAccountContext> options): base(options)
        {
        }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
