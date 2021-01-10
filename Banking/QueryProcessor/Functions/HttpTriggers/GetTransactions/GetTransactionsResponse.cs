using System.Collections.Generic;

namespace Banking.QueryProcessor.Functions.HttpTriggers.GetTransactions
{
    public class TransactionResponse
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }

        public TransactionResponse()
        {
        }
    }

    public class GetTransactionsResponse
    {
        public IEnumerable<TransactionResponse> Transactions { get; set; }
    }
}
