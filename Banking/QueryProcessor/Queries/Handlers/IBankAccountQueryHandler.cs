using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Queries.Queries;

namespace Banking.QueryProcessor.Queries.Handlers
{
    public interface IBankAccountQueryHandler
    {
        public Task<BankAccount> Handle(BankAccountQuery query);
    }
}
