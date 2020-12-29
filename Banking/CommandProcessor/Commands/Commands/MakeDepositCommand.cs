using System;

namespace Banking.CommandProcessor.Commands.Commands
{
    public class MakeDepositCommand: ICommand
    {
        public Guid AccountId { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }

        public MakeDepositCommand(Guid accountId, decimal amount, string description)
        {
            AccountId = accountId;
            Amount = amount;
            Description = description;
        }
    }
}
