using Banking.CommandProcessor.Entities;

namespace Banking.CommandProcessor.Commands.Commands
{
    public class MakeDepositCommand: ICommand
    {
        public AccountId AccountId { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }

        public MakeDepositCommand(AccountId accountId, decimal amount, string description)
        {
            AccountId = accountId;
            Amount = amount;
            Description = description;
        }
    }
}
