using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;
using Banking.QueryProcessor.Repository;
using MediatR;
using System.Linq;

namespace Banking.QueryProcessor.Queries.Handlers
{
    public class TransactionQuery : IRequest<List<Transaction>>
    {
        public string BankAccountId { get; private set; }

        public TransactionQuery(string bankAccountId)
        {
            BankAccountId = bankAccountId;
        }
    }

    public class TransactionQueryHandler: IRequestHandler<TransactionQuery, List<Transaction>>
    {
        private readonly IRepositoryFacade repositoryFacade;

        public TransactionQueryHandler(IRepositoryFacade repositoryFacade)
        {
            this.repositoryFacade = repositoryFacade;
        }

        public async Task<List<Transaction>> Handle(TransactionQuery request, CancellationToken cancellationToken)
        {
            var transactions = await repositoryFacade
                .TransactionsRepository
                .Get(transaction => transaction.BankAccount.Id == request.BankAccountId);

            return transactions.ToList();
        }
    }
}
