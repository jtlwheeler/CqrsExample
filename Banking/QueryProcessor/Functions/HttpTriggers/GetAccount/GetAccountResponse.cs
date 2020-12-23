namespace Banking.QueryProcessor.Functions.HttpTriggers.GetAccount
{
    public class AccountResponse
    {
        public string AccountId { get; private set; }
        public string AccountHolderName { get; private set; }
        public decimal Balance { get; private set; }

        public AccountResponse(string accountId, string accountHolderName, decimal balance)
        {
            AccountId = accountId;
            AccountHolderName = accountHolderName;
            Balance = balance;
        }
    }
}
