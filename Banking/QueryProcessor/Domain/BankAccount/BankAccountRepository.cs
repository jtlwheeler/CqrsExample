using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Banking.QueryProcessor.Domain.BankAccount
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly BankAccountContext context;

        public BankAccountRepository(BankAccountContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
        }

        public async Task Save(BankAccount bankAccount)
        {
            context.Add(bankAccount);
            await context.SaveChangesAsync();
        }

        public async Task<BankAccount> Get(string id)
        {
            return await context.BankAccounts.FindAsync(id);
        }

        public async Task Update(BankAccount bankAccount)
        {
            context.Update(bankAccount);
            await context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetTransactions(string id)
        {
            return await context.Transactions
                .Where(transaction => transaction.BankAccount.Id == id)
                .ToListAsync();
        }
    }
}
