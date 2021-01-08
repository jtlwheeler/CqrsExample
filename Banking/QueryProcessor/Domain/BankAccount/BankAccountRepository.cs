using System.Threading.Tasks;

namespace Banking.QueryProcessor.Domain.BankAccount
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly BankAccountContext context;

        public BankAccountRepository(BankAccountContext context)
        {
            this.context = context;
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
    }
}
