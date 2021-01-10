using System.Threading;
using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Repository;
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
        private readonly IRepositoryFacade repositoryFacade;

        public BankAccountQueryHandler(IRepositoryFacade repositoryFacade)
        {
            this.repositoryFacade = repositoryFacade;
        }

        public async Task<BankAccount> Handle(BankAccountQuery request, CancellationToken cancellationToken)
        {
            return await repositoryFacade.BankAccountRepository.Get(request.Id);
        }
    }
}
