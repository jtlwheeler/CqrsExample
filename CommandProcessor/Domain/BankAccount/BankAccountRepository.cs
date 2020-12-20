using System.Threading.Tasks;

namespace CommandProcessor.Domain.BankAccount
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
    }
}
