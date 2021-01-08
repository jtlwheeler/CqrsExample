using System.Collections.Generic;

namespace Banking.QueryProcessor.Domain.BankAccount
{
    public class BankAccount
    {
        public string Id { get; set; }
        public string AccountHolderName { get; set; }
        public decimal Balance { get; set; }

        public List<Transaction> Transactions { get; } = new List<Transaction>();
    }
}
