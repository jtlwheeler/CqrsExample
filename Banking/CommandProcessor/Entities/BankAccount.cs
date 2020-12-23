using System;
using Banking.Events;

namespace Banking.CommandProcessor.Entities
{
    public class BankAccount: Entity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }

        private int _nextVersionToAssign = 1;
        private int nextVersionToAssign => _nextVersionToAssign++;

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
                nextVersionToAssign
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

        public void MakeDeposit(string description, decimal amount)
        {
            var @event = new DepositMadeEvent(
                description,
                amount,
                Id,
                nextVersionToAssign
            );
            Apply(@event);
        }
    }
}
