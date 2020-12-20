using Microsoft.EntityFrameworkCore;

namespace CommandProcessor.Domain.BankAccount
{
    public class BankAccountContext : DbContext
    {
        private readonly string accountEndpoint;
        private readonly string accountKey;
        private readonly string databaseName = "BankDB";

        public BankAccountContext(string accountEndpoint, string accountKey)
        {
            this.accountEndpoint = accountEndpoint;
            this.accountKey = accountKey;
        }
        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                accountEndpoint,
                accountKey,
                databaseName
            );
        }
    }
}
