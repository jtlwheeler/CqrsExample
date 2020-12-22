using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Queries.Queries;

namespace Banking.QueryProcessor.Queries.Handlers
{
    public class BankAccountQueryHandler: IBankAccountQueryHandler
    {
        private readonly IBankAccountRepository bankAccountRepository;

        public BankAccountQueryHandler(IBankAccountRepository bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        public async Task<BankAccount> Handle(BankAccountQuery query)
        {
            return await bankAccountRepository.Get(query.Id);
        }
    }
}
