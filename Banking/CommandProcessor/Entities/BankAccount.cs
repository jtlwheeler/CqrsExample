using Banking.Events;

namespace Banking.CommandProcessor.Entities
{
    public class BankAccount: AggregateRoot
    {
        public AccountId Id { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }

        public void Open(string name)
        {
            if (Id != default)
            {
                return;
            }

            var newAccountId = new AccountId();
            var @event = new BankAccountCreatedEvent(
                name,
                newAccountId.Value,
                NextEventVersionToAssign
            );
            Apply(@event);
        }

        protected override void When(IEvent @event)
        {
            switch (@event)
            {
                case BankAccountCreatedEvent accountCreatedEvent:
                    Name = accountCreatedEvent.Name;
                    Id = new AccountId(accountCreatedEvent.EntityId);
                    break;
                case DepositMadeEvent e:
                    Balance += e.Amount;
                    break;
            }
        }

        public DepositId MakeDeposit(string description, decimal amount)
        {
            if (Id == default)
            {
                throw new EntityException("Bank account must be opened before making a deposit.");
            }

            var depositId = new DepositId();

            var @event = new DepositMadeEvent(
                depositId.Value,
                description,
                amount,
                Id.Value,
                NextEventVersionToAssign
            );
            Apply(@event);

            return depositId;
        }
    }
}
