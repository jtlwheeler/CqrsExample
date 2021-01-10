using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;

namespace Banking.QueryProcessor.Repository
{
    public interface IRepositoryFacade
    {
        public IRepository<Transaction> TransactionsRepository { get; }
        public IRepository<BankAccount> BankAccountRepository {get; }
        public Task Save();
    }
}
