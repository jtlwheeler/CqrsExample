using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;

namespace Banking.QueryProcessor.Repository
{
    public class RepositoryFacade: IRepositoryFacade
    {
        private readonly BankContext context;
        public IRepository<Transaction> TransactionsRepository { get; private set; }
        public IRepository<BankAccount> BankAccountRepository {get; private set;}

        public RepositoryFacade(BankContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();

            TransactionsRepository = new Repository<Transaction>(context);
            BankAccountRepository = new Repository<BankAccount>(context);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
