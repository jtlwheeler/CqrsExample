namespace Banking.QueryProcessor.Functions.HttpTriggers.GetTransactions
{
    public class GetTransactionsResponse
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }

        public GetTransactionsResponse()
        {
        }
    }
}
