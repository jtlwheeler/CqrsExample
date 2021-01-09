namespace Banking.QueryProcessor.Domain.BankAccount
{
    public enum TransactionType
    {
        Deposit
    }

    public class Transaction
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }

        public BankAccount BankAccount { get; set; }

        public Transaction()
        {
        }
    }
}
