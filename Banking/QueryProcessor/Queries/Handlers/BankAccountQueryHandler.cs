using System.Threading;
using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;
using MediatR;

namespace Banking.QueryProcessor.Queries.Handlers
{
    public class BankAccountQuery : IRequest<BankAccount>
    {
        public string Id { get; private set; }
        public BankAccountQuery(string id)
        {
            Id = id;
        }
    }

    public class BankAccountQueryHandler: IRequestHandler<BankAccountQuery, BankAccount>
    {
        private readonly IBankAccountRepository bankAccountRepository;

        public BankAccountQueryHandler(IBankAccountRepository bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        public async Task<BankAccount> Handle(BankAccountQuery request, CancellationToken cancellationToken)
        {
            return await bankAccountRepository.Get(request.Id);
        }
    }
}
