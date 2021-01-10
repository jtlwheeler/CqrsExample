using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Banking.QueryProcessor.Domain.BankAccount;
using MediatR;

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
        private readonly IBankAccountRepository _bankAccountRepository;

        public TransactionQueryHandler(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<List<Transaction>> Handle(TransactionQuery request, CancellationToken cancellationToken)
        {
            return await _bankAccountRepository.GetTransactions(request.BankAccountId);
        }
    }
}
