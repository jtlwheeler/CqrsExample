using System.Collections.Generic;
using System.Threading.Tasks;

namespace Banking.QueryProcessor.Domain.BankAccount
{
    public interface IBankAccountRepository
    {
        public Task Save(BankAccount bankAccount);
        public Task<BankAccount> Get(string id);
        public Task Update(BankAccount bankAccount);
        public Task<List<Transaction>> GetTransactions(string id);
    }
}
