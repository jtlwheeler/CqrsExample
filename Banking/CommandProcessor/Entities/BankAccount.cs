using System;
using Banking.Events;

namespace Banking.CommandProcessor.Entities
{
    public class BankAccount: Entity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }

        public void Open(string name)
        {
            if (Id != default)
            {
                return;
            }

            var newAccountId = Guid.NewGuid();
            var @event = new BankAccountCreatedEvent(
                name,
                newAccountId,
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
                    Id = accountCreatedEvent.EntityId;
                    break;
                case DepositMadeEvent e:
                    Balance += e.Amount;
                    break;
            }
        }

        public Guid MakeDeposit(string description, decimal amount)
        {
            if (Id == default)
            {
                throw new EntityException("Bank account must be opened before making a deposit.");
            }

            var depositId = Guid.NewGuid();

            var @event = new DepositMadeEvent(
                depositId,
                description,
                amount,
                Id,
                NextEventVersionToAssign
            );
            Apply(@event);

            return depositId;
        }
    }
}
